using Microsoft.AspNetCore.Mvc;
using itsc_dotnet_practice.Models.Dtos;
using itsc_dotnet_practice.Services.Interface;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Serilog.Core;
using System;

namespace itsc_dotnet_practice.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;
    private readonly ILogger<AuthController> _logger;
    public AuthController(IAuthService authService, ILogger<AuthController> logger) 
    {
        _authService = authService;
        _logger = logger;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequestDto login)
    {
        var user = await _authService.Authenticate(login);
        if (user == null) return Unauthorized("Invalid credentials");
        var token = _authService.GenerateJwtToken(user);
        _logger.LogDebug("User {Username} logged in successfully", user.Username);
        return Ok(new { token });
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequestDto register)
    {
        var result = await _authService.Register(register);
        if (result == null) return BadRequest("Username already exists");

        return Ok("User registered successfully");
    }

}