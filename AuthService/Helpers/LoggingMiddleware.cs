using Microsoft.AspNetCore.Mvc.ModelBinding;
using NLog;
using System.Diagnostics;
using System.Security.Claims;

namespace AuthService.Helpers
{
    public class LoggingMiddleware
    {
        private Logger _logger;
        private readonly RequestDelegate _next;
        public LoggingMiddleware(RequestDelegate next)
        {
            _logger = LogManager.GetCurrentClassLogger();
            _next = next;
        }
        public Task InvokeAsync(HttpContext context)
        {
            var watch = new Stopwatch();
            watch.Start();
            context.Response.OnStarting(() =>
            {
                watch.Stop();
                var userName = context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                var actionName = context.Request.RouteValues["action"];
                var responseTime = watch.ElapsedMilliseconds;
                var eventInfo = new LogEventInfo(NLog.LogLevel.Info, _logger.Name, "Message from logging middleware");
                eventInfo.Properties["userName"] = userName;
                eventInfo.Properties["responseTime"] = responseTime.ToString();
                eventInfo.Properties["statusCode"] = context.Response.StatusCode.ToString();
                _logger.Log(eventInfo);
                return Task.CompletedTask;
            });
            return this._next(context);
        }
    }
}
