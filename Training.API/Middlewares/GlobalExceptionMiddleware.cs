using System.Net;
using System.Text.Json;

namespace Training.API.Middlewares
{
    public class GlobalExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public GlobalExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                // 繼續往下一個 middleware 或 endpoint
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private static async Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            var response = new
            {
                Code = 500,
                Message = "Internal Server Error",
                Error = ex.Message
            };

            var json = JsonSerializer.Serialize(response);

            Console.WriteLine($"[ERROR] {ex.Message}");

            await context.Response.WriteAsync(json);
        }
    }
}
