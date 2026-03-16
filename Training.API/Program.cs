using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Training.API.Middlewares;
using Training.Core;
using Training.Core.DTOs;
using Mapster;
using FluentValidation;
using Training.Core.Validators;
using Microsoft.EntityFrameworkCore;
using Training.Core.Models;

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

builder.Services.AddDbContext<BookStoreDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("BookStoreDB")));

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


app.Run();