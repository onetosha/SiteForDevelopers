using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace AuthService.Controllers
{
    [ApiController]
    [Route("api/toggle-logging")]
    public class ToggleLoggingController : ControllerBase
    {
        private static bool _loggingEnabled = true; // Default logging enabled

        [HttpPost]
        public IActionResult ToggleLogging([FromBody] LoggingToggleRequest request)
        {
            _loggingEnabled = request.LoggingEnabled;
            return Ok(new { LoggingEnabled = _loggingEnabled });
        }

        public static bool IsLoggingEnabled()
        {
            return _loggingEnabled;
        }
    }

    public class LoggingToggleRequest
    {
        public bool LoggingEnabled { get; set; }
    }
}