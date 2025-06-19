using KBIPMobileBackend.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace KBIPMobileBackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ScheduleController : ControllerBase
    {
        private readonly ApplicationDbContext _db;
        public ScheduleController(ApplicationDbContext db) => _db = db;

        [HttpGet("{groupNumber}")]
        public async Task<IActionResult> GetByGroup(string groupNumber)
        {
            var sched = await _db.ScheduleEntries
                .Where(e => e.GroupNumber == groupNumber)
                .OrderBy(e => e.DateTime)
                .ToListAsync();
            return Ok(sched);
        }
    }
}