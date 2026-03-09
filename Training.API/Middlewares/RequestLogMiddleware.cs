using System.Diagnostics;

namespace Training.API.Middlewares
{
    public class RequestLogMiddleware
    {
        private readonly RequestDelegate _next;

        public RequestLogMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // 記錄開始時間
            var stopwatch = Stopwatch.StartNew();

            // 呼叫下一個 middleware
            await _next(context);

            // 停止計時
            stopwatch.Stop();
            
            var elapsedMs = stopwatch.ElapsedMilliseconds;
            
            Console.WriteLine($"[API Timing] {context.Request.Method} {context.Request.Path} took {elapsedMs} ms");
        }
    }
}
