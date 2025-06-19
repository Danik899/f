using KBIPMobileBackend.Data;
using KBIPMobileBackend.Models;
using KBIPMobileBackend.Models.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace KBIPMobileBackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MaterialsController : ControllerBase
    {
        private readonly ApplicationDbContext _db;
        public MaterialsController(ApplicationDbContext db) => _db = db;

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] Language? lang)
        {
            var q = _db.Materials.AsQueryable();
            if (lang.HasValue)
            {
                q = q.Where(m => m.Language == lang);
            }

            var list = await q.ToListAsync();
            return Ok(list);
        }
    }
}