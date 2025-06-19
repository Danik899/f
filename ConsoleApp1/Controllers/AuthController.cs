using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.JwtBearer;

// 1) Data: ApplicationDbContext
namespace MyApp.Data
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<MyApp.Models.User> Users { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        { }
    }
}

// 2) Models: User entity
namespace MyApp.Models
{
    public class User
    {
        public int    Id           { get; set; }
        public string Username     { get; set; }
        public string Email        { get; set; }
        public string PasswordHash { get; set; }
        public string FullName     { get; set; }
        public string GroupNumber  { get; set; }
    }
}

// 3) DTOs: LoginRequest & RegisterRequest
namespace MyApp.DTOs
{
    public class LoginRequest
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }

    public class RegisterRequest
    {
        public string Username    { get; set; }
        public string Password    { get; set; }
        public string FullName    { get; set; }
        public string GroupNumber { get; set; }
        public string Email { get; set; }
    }
}

// 4) Services: JWT generation
namespace MyApp.Services
{
    public interface IJwtService
    {
        string GenerateToken(MyApp.Models.User user);
    }

    public class JwtService : IJwtService
    {
        private readonly string _secret;
        private readonly int    _expiryMinutes;

        public JwtService(IConfiguration config)
        {
            _secret        = config["Jwt:Secret"];
            _expiryMinutes = int.Parse(config["Jwt:ExpiryMinutes"]);
        }

        public string GenerateToken(MyApp.Models.User user)
        {
            var key    = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secret));
            var creds  = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var now    = DateTime.UtcNow;
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim("username", user.Username),
                new Claim("email", user.Email ?? ""),
            };

            var token = new JwtSecurityToken(
                issuer:     "MyApp",
                audience:   "MyApp",
                claims:     claims,
                notBefore:  now,
                expires:    now.AddMinutes(_expiryMinutes),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}

// 5) Controllers: AuthController
namespace MyApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly MyApp.Data.ApplicationDbContext _context;
        private readonly MyApp.Services.IJwtService      _jwtService;

        public AuthController(
            MyApp.Data.ApplicationDbContext context,
            MyApp.Services.IJwtService      jwtService)
        {
            _context    = context;
            _jwtService = jwtService;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] MyApp.DTOs.LoginRequest request)
        {
            var user = _context.Users
                .FirstOrDefault(u => u.Username == request.Username);

            if (user == null ||
                !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
                return Unauthorized("Invalid credentials");

            var token = _jwtService.GenerateToken(user);
            return Ok(new { token });
        }

        [HttpPost("register")]
        public IActionResult Register([FromBody] MyApp.DTOs.RegisterRequest request)
        {
            if (_context.Users.Any(u => u.Username == request.Username))
                return BadRequest("Username already in use");

            var passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);

            var newUser = new MyApp.Models.User
            {
                Username     = request.Username,
                Email        = request.Email,
                PasswordHash = passwordHash,
                FullName     = request.FullName,
                GroupNumber  = request.GroupNumber
            };

            _context.Users.Add(newUser);
            _context.SaveChanges();

            var token = _jwtService.GenerateToken(newUser);
            return Ok(new { token });
        }
    }
}
