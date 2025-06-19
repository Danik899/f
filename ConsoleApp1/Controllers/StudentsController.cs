using KBIPMobileBackend.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace KBIPMobileBackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StudentsController : ControllerBase
    {
        private readonly ApplicationDbContext _db;
        public StudentsController(ApplicationDbContext db) => _db = db;

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var st = await _db.Students.FindAsync(id);
            if (st == null) return NotFound();
            return Ok(st);
        }

        [HttpGet("me")]
        public async Task<IActionResult> Me()
        {
            var uid = int.Parse(User.FindFirst("id").Value);
            var st = await _db.Students
                .SingleOrDefaultAsync(s => s.Id == uid);
            return Ok(st);
        }
    }
}