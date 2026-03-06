using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Training.Core;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<Class1>();
builder.Services.AddSingleton<Linq>();
builder.Services.AddSingleton<Async>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();



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


app.Run();