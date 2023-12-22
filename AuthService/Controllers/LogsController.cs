using AuthService.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AuthService.Controllers
{
    [ApiController]
    [Authorize]
    [Route("[controller]")]
    public class LogsController : Controller
    {
        private readonly AppDBContext _dbContext;
        public LogsController(AppDBContext dbContext) 
        {
            _dbContext = dbContext;
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var logs = await _dbContext.logs.ToListAsync();
            return Ok(logs);
        }
    }
}
