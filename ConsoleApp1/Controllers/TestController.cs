using KBIPMobileBackend.Data;
using Microsoft.AspNetCore.Mvc;


namespace KBIPMobileBackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TestController : ControllerBase
    {
        private readonly ApplicationDbContext _db;

        public TestController(ApplicationDbContext db)
        {
            _db = db;
        }

        [HttpGet("users-count")]
        public IActionResult GetUsersCount()
        {
            var count = _db.Users.Count();
            return Ok(new { Count = count });
        }
    }
}