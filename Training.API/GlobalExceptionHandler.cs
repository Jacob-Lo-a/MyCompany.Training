using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Net.Sockets;

namespace Training.API
{
    public class GlobalExceptionHandler : IExceptionHandler 
    {
        private readonly ILogger<GlobalExceptionHandler> _logger;
        public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger) 
        {
            _logger = logger;        
        }

        public async ValueTask<bool> TryHandleAsync(
            HttpContext httpContext,
            Exception exception,
            CancellationToken cancellationToken)
        {

            ProblemDetails problemDetails = exception switch
            {
                InsufficientStockException stockEx => new ProblemDetails
                {
                    Title = "庫存不足",
                    Detail = stockEx.Message,
                    Status = (int)HttpStatusCode.BadRequest,
                    Instance = httpContext.Request.Path
                },
                BookNotFoundException ex => new ProblemDetails
                {
                    Title = "書籍不存在",
                    Detail = ex.Message,
                    Status = (int)HttpStatusCode.BadRequest,
                    Instance = httpContext.Request.Path
                },

                _ => HandleUnknownException(exception, httpContext)
            };

            httpContext.Response.StatusCode = problemDetails.Status!.Value;
            httpContext.Response.ContentType = "application/json";

            await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);

            return true;
        }

        private ProblemDetails HandleUnknownException(Exception exception, HttpContext httpContext)
        {
            _logger.LogError(exception, "系統發生未預期錯誤");

            return new ProblemDetails
            {
                Title = "系統發生錯誤",
                Detail = exception.Message,
                Status = (int)HttpStatusCode.InternalServerError,
                Instance = httpContext.Request.Path
            };


        }
    }
}
