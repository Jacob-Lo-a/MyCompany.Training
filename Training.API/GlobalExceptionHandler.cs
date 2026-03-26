using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using System.Net;

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
            
            ProblemDetails problemDetails; 

            if (exception is InsufficientStockException stockEx)
            {
                problemDetails = new ProblemDetails
                {
                    Title = "庫存不足",
                    Detail = stockEx.Message,
                    Status = (int)HttpStatusCode.BadRequest,
                    Instance = httpContext.Request.Path
                };
            }
            else
            {
                _logger.LogError(exception, "系統發生未預期錯誤");

                problemDetails = new ProblemDetails
                {
                    Title = "系統發生錯誤",
                    Detail = exception.Message,
                    Status = (int)HttpStatusCode.InternalServerError,
                    Instance = httpContext.Request.Path
                };

            }
               

            httpContext.Response.StatusCode = problemDetails.Status.Value;
            httpContext.Response.ContentType = "application/json";

            await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);

            return true;


        }
    }
}
