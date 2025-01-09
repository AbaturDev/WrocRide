
using System.Diagnostics;

namespace WrocRide.Middleware
{
    public class RequestLoggingMiddleware : IMiddleware
    {
        private readonly ILogger<RequestLoggingMiddleware> _logger;
        private Stopwatch timer;

        public RequestLoggingMiddleware(ILogger<RequestLoggingMiddleware> logger)
        {
            _logger = logger;
            timer = new Stopwatch();
        }
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            timer.Start();

            string requestMessage = $"Request: [{context.Request.Method}] at {context.Request.Path} from IP: {context.Connection.RemoteIpAddress}";
            _logger.LogInformation(requestMessage);

            await next.Invoke(context);

            timer.Stop();

            var responseMessage = $"Request: [{context.Request.Method}] at {context.Request.Path} took {timer.ElapsedMilliseconds}ms";
            _logger.LogInformation(responseMessage);

        }
    }
}
