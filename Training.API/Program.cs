using FluentValidation;
using Hangfire;
using Mapster;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using NLog.Web;
using Polly;
using Polly.Extensions.Http;
using System.Diagnostics;
using System.Security.Claims;
using System.Text;
using Training.API.Exceptions;
using Training.API.Middlewares;
using Training.API.Repositories;
using Training.API.Services;
using Training.Core;
using Training.Core.DTOs;
using Training.Core.interfaces;
using Training.Core.Models;
using Training.Core.Repositories;
using Training.Core.Services;
using Training.Core.Validators;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();
// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    // JWT 設定
    options.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Description = "請輸入 JWT Token（格式：Bearer {token}）"
    });

    options.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });

    
});



builder.Services.AddSingleton<Class1>();
builder.Services.AddSingleton<Linq>();
builder.Services.AddSingleton<Async>();
builder.Services.AddScoped<IGuidGenerator, MyGuidGenerator>();
builder.Services.Configure<Emailsettings>(
    builder.Configuration.GetSection("EmailSettings")
);
builder.Services.AddValidatorsFromAssemblyContaining<Program>();
builder.Services.AddScoped<IValidator<RegisterRequestDTO>, RegisterValidator>();

builder.Logging.ClearProviders();
builder.Host.UseNLog();

builder.Services.AddDbContext<BookStoreDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("BookStoreDB"))
    );

//忽略循環
builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.ReferenceHandler =
        System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
});
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();

builder.Services.AddScoped<IBookRepository, BookRepository>();
builder.Services.AddScoped<IBookService, BookService>();

builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();


builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

builder.Services.AddAuthentication()
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("e3b0c44298fc1c149afbf4c8996fb92427ae41e4649b934ca495991b7852b855")),
            NameClaimType = ClaimTypes.NameIdentifier

        };
    });



// polly套件 失敗了自動重試，最多重試 3 次，每次間隔 2 秒
static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy()
{
    return HttpPolicyExtensions
        .HandleTransientHttpError()
        .OrResult(msg => msg.StatusCode == System.Net.HttpStatusCode.NotFound)
        .WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));
}
builder.Services.AddHttpClient("CwaClient", client =>
{
    client.BaseAddress = new Uri("https://opendata.cwa.gov.tw/");
    client.Timeout = TimeSpan.FromSeconds(10);
})
.AddPolicyHandler(GetRetryPolicy());

builder.Services.AddScoped<ITaipeiWeatherService, TaipeiWeatherService>();

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: "MyPolicy",
                      policy =>
                      {
                          policy.WithOrigins("http://localhost:5500")
                                .AllowAnyHeader()
                                .AllowAnyMethod();
                      });
});

builder.Services.Configure<SftpSettings>(
    builder.Configuration.GetSection("SftpSettings"));

builder.Services.AddScoped<ISftpService, SftpService>();

builder.Services.AddHangfire(config =>
    config.UseSqlServerStorage(
        builder.Configuration.GetConnectionString("BookStoreDB")));

builder.Services.AddScoped<EmailService>();

builder.Services.AddHangfireServer();

var app = builder.Build();



// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseMiddleware<GlobalExceptionMiddleware>();
app.UseMiddleware<RequestLogMiddleware>();
app.UseCors("MyPolicy");

app.UseExceptionHandler(_ => { });
app.UseAuthentication(); // 啟用 JWT 驗證
app.UseAuthorization();

app.UseHangfireDashboard();
app.MapControllers();

app.MapGet("/system-info", (Class1 systemInfo) =>
{
    return Results.Ok(systemInfo.GetOSAndDateTime());
})
.WithName("GetSystemInfo")
.WithOpenApi();

app.MapGet("/linq-test", (Linq linq) =>
{
    linq.test();
    return Results.Ok("Linq test executed successfully");
})
.WithName("LinqTest")
.WithOpenApi();

app.MapGet("/async-test", async (Async a, CancellationToken cancellationToken) =>
{
    Stopwatch stopWatch = new Stopwatch(); //判斷程式的運行時間
    stopWatch.Start();

    var r1 = await a.DownloadDataAsync(1, cancellationToken);
    var r2 = await a.DownloadDataAsync(2, cancellationToken);
    var r3 = await a.DownloadDataAsync(3, cancellationToken);

    stopWatch.Stop();
    
    return Results.Ok(new
    {
        Results = new[] { r1, r2, r3 },
        Time = stopWatch.Elapsed.TotalSeconds
    });

})
.WithName("AsyncTest")
.WithOpenApi();

app.MapGet("/whenAll-test", async (Async a, CancellationToken cancellationToken) =>
{
    Stopwatch stopWatch = new Stopwatch(); //判斷程式的運行時間

    stopWatch.Start();
    Task<string> r1 = a.DownloadDataAsync(1, cancellationToken);
    Task<string> r2 = a.DownloadDataAsync(2, cancellationToken);
    Task<string> r3 = a.DownloadDataAsync(3, cancellationToken);


    var results = await Task.WhenAll(r1, r2, r3);
    stopWatch.Stop();

    return Results.Ok(new
    {
        Results = results,
        Time = stopWatch.Elapsed.TotalSeconds
    });

})
.WithName("whenAll")
.WithOpenApi();

app.MapGet("/error-test", () =>
{
    throw new Exception("Something went wrong!");
})
.WithName("errorTest")
.WithOpenApi();


app.MapGet("/guid", (IGuidGenerator g1, IGuidGenerator g2) =>
{
    return new
    {
        Generator1 = g1.GetGuid(),
        Generator2 = g2.GetGuid()
    };
})
.WithName("guidTest")
.WithOpenApi();

app.MapGet("/email", (IOptions<Emailsettings> options) =>
{
    return options.Value;
})
.WithName("emailTest")
.WithOpenApi();

app.MapGet("/DTO", () =>
{
    var user = new Users
    {
        Id = 1,
        Account = "testUser",
        Password = "123456",
        CreatedDate = DateTime.Now
    };
    var result = user.Adapt<UserResponseDTO>();

    return result;
})
.WithName("DTOTest")
.WithOpenApi();

app.MapPost("/register", async (
    RegisterRequestDTO request,
    IValidator<RegisterRequestDTO> validator) =>
{
    var result = await validator.ValidateAsync(request);

    if (!result.IsValid)
    {
        var errors = result.Errors
            .GroupBy(e => e.PropertyName)
            .ToDictionary(
                g => g.Key,
                g => g.Select(e => e.ErrorMessage).ToArray()
            );

        return Results.ValidationProblem(errors);
    }

    return Results.Ok(new { message = "註冊成功" });

})
.WithName("register")
.WithOpenApi();

app.MapGet("/books", (BookStoreDbContext db) =>
{
    return db.Books.ToList();
})
.WithName("books")
.WithOpenApi();


app.MapGet("/authors-nlog", async (BookStoreDbContext db) =>
{
    var authors = await db.Authors
        .Include(a => a.Books) 
        .ToListAsync();
    
    var result = new List<object>();

    foreach (var author in authors)
    {
        result.Add(new
        {
            author.Id,
            author.Name,
            BookCount = author.Books.Count
        });
    }
    
    return result;
})
.WithName("authors-nlog")
.WithOpenApi();

app.MapGet("/AsNoTracking", async (BookStoreDbContext db) =>
{
    var books = await db.Books
        .AsNoTracking()
        .Include(b => b.Author)
        .ToListAsync();

    return books;
})
.WithName("AsNoTracking")
.WithOpenApi();

app.MapPost("/Transaction", async (
    BuyBookRequest request,
    BookStoreDbContext _dbContext,
    ILogger<Program> _logger) =>
{
    using var transaction = await _dbContext.Database.BeginTransactionAsync();

    try
    {
        var book = await _dbContext.Books
            .FirstOrDefaultAsync(b => b.Id == request.BookId);

        if (book == null)
            return Results.NotFound("Book not found");

        if (book.Stock < request.Quantity)
            throw new Exception("庫存不足");

        book.Stock -= request.Quantity;
        
        // 建訂單
        var order = new Order
        {
            OrderNumber = $"ORD-{DateTime.Now:yyyyMMddHHmmss}",
            UserId = request.UserId,
            TotalAmount = book.Price * request.Quantity,
            Status = "Pending",
            CreatedAt = DateTime.Now
        };

        _dbContext.Orders.Add(order);

        _dbContext.OrderItems.Add(new OrderItem
        {
            BookId = book.Id,
            Quantity = request.Quantity,
            UnitPrice = book.Price,
            Order = order
        });
        
        await _dbContext.SaveChangesAsync(); 
        await transaction.CommitAsync(); //全部成功才提交

        return Results.Ok(order);
    }
    catch (Exception ex)
    {
        await transaction.RollbackAsync(); // 出錯立刻還原
        _logger.LogError(ex, "交易失敗，資料已 Rollback");
        return Results.Problem("交易失敗");
    }
    
})
.WithName("Transaction")
.WithOpenApi();


app.Run(); 
