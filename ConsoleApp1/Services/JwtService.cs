using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using KBIPMobileBackend.Models;

public class JwtService : IJwtService
{
    private readonly IConfiguration _cfg;
    private readonly byte[] _key;

    public JwtService(IConfiguration cfg)
    {
        _cfg = cfg;
        _key = Encoding.UTF8.GetBytes(_cfg["Jwt:Secret"]!);
    }

    public string GenerateToken(User user)
    {
        var creds = new SigningCredentials(new SymmetricSecurityKey(_key), SecurityAlgorithms.HmacSha256);
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Username),
            new Claim("studentId", user.StudentId?.ToString() ?? ""),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var token = new JwtSecurityToken(
            issuer: _cfg["Jwt:Issuer"],
            claims: claims,
            expires: DateTime.UtcNow.AddHours(2),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}