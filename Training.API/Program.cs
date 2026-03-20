using FluentValidation;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using NLog;
using NLog.Web;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Training.API.Middlewares;
using Training.Core;
using Training.Core.DTOs;
using Training.Core.interfaces;
using Training.Core.Models;
using Training.Core.Repositories;
using Training.Core.Services;
using Training.Core.Validators;

var builder = WebApplication.CreateBuilder(args);



// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();



builder.Services.AddSingleton<Class1>();
builder.Services.AddSingleton<Linq>();
builder.Services.AddSingleton<Async>();
builder.Services.AddScoped<IGuidGenerator, MyGuidGenerator>();
builder.Services.Configure<EmailOptions>(
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

builder.Services.AddScoped<IBookRepository, BookRepository>();
builder.Services.AddScoped<IBookService, BookService>();

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

app.MapGet("/email", (IOptions<EmailOptions> options) =>
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

app.MapGet("/repository", (IBookService bookService) =>
{
    var result = bookService.GetDiscountBooks();
    return result;
})
.WithName("repository")
.WithOpenApi();

app.MapGet("/GetBooks", async (
    [AsParameters] BookQueryParameters parameters,
    BookStoreDbContext context,
    ILogger<Program> logger,
    CancellationToken ct) =>
{ 
    try
    {
        parameters.PageSize = Math.Min(parameters.PageSize, 50);

        IQueryable<Book> query = context.Books.AsQueryable();

        if (!string.IsNullOrWhiteSpace(parameters.Keyword))
        {
            query = query.Where(b => b.Title.Contains(parameters.Keyword));
        }
        
        query = query.OrderBy(b => b.Id);

        var totalCount = await query.CountAsync(ct);

        var books = await query
            .Skip((parameters.PageNumber - 1) * parameters.PageSize)
            .Take(parameters.PageSize)
            .ToListAsync(ct);
        
        return Results.Ok(new
        {
            Total = totalCount,
            PageNumber = parameters.PageNumber,
            PageSize = parameters.PageSize,
            Data = books
        });
    }
    catch (OperationCanceledException)
    {
        logger.LogWarning("使用者取消了耗時的書籍查詢");
        return Results.StatusCode(499); // 設定 http 狀態
    }
    catch (Exception ex)
    {
        logger.LogError(ex, "查詢書籍時發生未預期錯誤");
        return Results.Problem("系統發生錯誤"); 
    }
})
.WithName("GetBooks")
.WithOpenApi();

app.Run();