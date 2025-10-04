using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Services.Models;
using Services.Repositories;

namespace Services.Services;

public class UserService
{
    private readonly IUserRepository _users;
    private readonly IConfiguration _config;

    public UserService(IUserRepository users, IConfiguration config)
    {
        _users = users;
        _config = config;
    }

    // Register menerima username + password (role selalu Ranger)
    public async Task<User> RegisterAsync(string email, string username, string password)
    {
        // minimal cek
        if(string.IsNullOrWhiteSpace(email)) throw new ArgumentException("Email is required.");
        if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            throw new ArgumentException("Username and password are required.");

        if (password.Length < 6) // minimal cek, sesuaikan kebijakan
            throw new ArgumentException("Password must be at least 6 characters.");

        var existing = await _users.GetByEmailAsync(email);
        if (existing != null)
            throw new InvalidOperationException("email already exists.");

        var hashed = _users.HashPassword(password);

        var user = new User
        {
            Username = username,
            PasswordHash = hashed,
            Email = email,
            Role = "Ranger" // selalu Ranger
        };

        await _users.AddAsync(user);
        return user;
    }

    public async Task<string?> LoginAsync(string email, string password)
    {
        var user = await _users.GetByEmailAsync(email);
        if (user == null) return null;
        if (!_users.VerifyPassword(password, user.PasswordHash)) return null;

        return GenerateJwtToken(user);
    }

    private string GenerateJwtToken(User user)
    {
        var keyString = _config["Jwt:Key"] ?? throw new InvalidOperationException("JWT key missing");
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(keyString));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(ClaimTypes.Name, user.Username),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Role, user.Role),
            new Claim("uid", user.UserNID.ToString())
        };

        var expiresHours = int.TryParse(_config["Jwt:ExpiresHours"], out var h) ? h : 3;

        var token = new JwtSecurityToken(
            issuer: _config["Jwt:Issuer"],
            audience: _config["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddHours(expiresHours),
            signingCredentials: creds
        );

        var jwt = new JwtSecurityTokenHandler().WriteToken(token);
        return jwt;
    }

}
