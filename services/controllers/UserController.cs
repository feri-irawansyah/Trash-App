using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Services;

namespace Services.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly UserService _auth;

    public AuthController(UserService auth)
    {
        _auth = auth;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request)
    {
        var user = await _auth.RegisterAsync(request.Email, request.Username, request.Password);
        return Ok(new { user.Username, user.Role });
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        var token = await _auth.LoginAsync(request.Email, request.Password);
        if (token is null)
            return Unauthorized(new { message = "Invalid credentials" });

        // ✅ return token langsung (buat disimpan client)
        return Ok(new { message = "Login success", token });
    }

    [HttpPost("logout")]
    [Authorize]
    public IActionResult Logout()
    {
        // Karena JWT stateless, cukup biarkan client hapus token-nya
        return Ok(new { message = "Logout success (hapus token di client)" });
    }

    [HttpGet("session")]
    [Authorize]
    public IActionResult SessionCheck()
    {
        var username = User.Identity?.Name;
        var role = User.FindFirst(ClaimTypes.Role)?.Value;
        var email = User.FindFirst(ClaimTypes.Email)?.Value;

        return Ok(new
        {
            username,
            email,
            role,
            isAuthenticated = User.Identity?.IsAuthenticated ?? false
        });
    }
}

public record RegisterRequest(string Email, string Username, string Password);
public record LoginRequest(string Email, string Password);
