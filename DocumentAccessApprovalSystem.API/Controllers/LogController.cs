using DocumentAccessApprovalSystem.Infrastructure.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DocumentAccessApprovalSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LogController : ControllerBase
    {
        [HttpGet("notifications")]
        public IActionResult GetNotificationLogs([FromQuery] int count = 50)
        {
            var lines = FileLogger.ReadRecent("notifications.log", count);
            return Ok(lines);
        }

        [HttpGet("emails")]
        public IActionResult GetEmailLogs([FromQuery] int count = 50)
        {
            var lines = FileLogger.ReadRecent("emails.log", count);
            return Ok(lines);
        }
    }
}
