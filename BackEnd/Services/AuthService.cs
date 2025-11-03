using AutoMapper;
using Humanizer;
using itsc_dotnet_practice.Models;
using itsc_dotnet_practice.Models.Dtos;
using itsc_dotnet_practice.Repositories.Interface;
using itsc_dotnet_practice.Services.Interface;
using itsc_dotnet_practice.Utilities;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace itsc_dotnet_practice.Services;

public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepo;
    private readonly IConfiguration _config;

    public AuthService(IUserRepository userRepo, IConfiguration config)
    {
        _userRepo = userRepo;
        _config = config;
    }

    public async Task<User?> Authenticate(LoginRequestDto login)
    {
        return await _userRepo.GetUser(login.Username, login.Password);
    }

    public User? ValidateBasicAuth(string authHeader)
    {
        if (string.IsNullOrWhiteSpace(authHeader)) return null;

        var encoded = authHeader.Replace("Basic ", "");
        var decoded = Encoding.UTF8.GetString(Convert.FromBase64String(encoded));
        var parts = decoded.Split(':');
        if (parts.Length != 2) return null;

        //return await _userRepo.GetValidationBasicAuth(parts[0], parts[1]);
        var adminUsername = _config["ADMIN_USERNAME"];
        var adminPassword = _config["ADMIN_PASSWORD"];
        
        if (string.IsNullOrEmpty(adminUsername) || string.IsNullOrEmpty(adminPassword))
        {
            throw new System.Exception("ADMIN_USERNAME or ADMIN_PASSWORD is not set.");
        }
        if (adminUsername != parts[0] || adminPassword != parts[1])
        {
            return null;
        }
        return new User
        {
            Username = parts[0],
            Role = "Admin"
        };
    }

    public string GenerateJwtToken(User user)
    {
        var isAdmin = user.Role.Equals("Admin", StringComparison.OrdinalIgnoreCase);

        var claims = new List<Claim>
    {
        new Claim("userId", isAdmin ? "Admin" : user.Id.ToString()),
        new Claim(ClaimTypes.Name, user.Username),
        new Claim(ClaimTypes.Role, user.Role),
        new Claim("username", user.Username),
        new Claim("role", user.Role)
    };

        if (!isAdmin)
        {
            claims.AddRange(new[]
            {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim("fullName", user.FullName),
            new Claim("phone", user.Phone)
        });
        }

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JWT_KEY"]!));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _config["JWT_ISSUER"],
            audience: _config["JWT_AUDIENCE"],
            claims: claims,
            expires: DateTime.Now.AddHours(5),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public async Task<User> Register(RegisterRequestDto register)
    {
        User? existingUser = await _userRepo.GetUserByUsername(register.Username);
        if (existingUser != null)
        {
            throw new Exception("Username already exists");
        }
        if (register.Password != register.ConfirmPassword)
        {
            throw new Exception("Passwords do not match");
        }
        User newUser = new User
        {
            Username = register.Username,
            FullName = register.FullName,
            Phone = register.Phone,
            Password = EncryptionUtility.HashPassword(register.Password),
            Role = "User"
        };
        return await _userRepo.CreateUser(newUser);
    }
}