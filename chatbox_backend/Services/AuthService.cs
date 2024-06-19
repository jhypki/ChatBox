using Repositories;
using Models;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using BCrypt.Net;

namespace Services;

public class AuthService : IAuthService
{
    private readonly IConfiguration _configuration;
    private readonly IAuthRepository _authRepository;
    public AuthService(IAuthRepository authRepository, IConfiguration configuration)
    {
        _authRepository = authRepository;
        _configuration = configuration;
    }

    public async Task<User> Register(User user)
    {
        if (user.Email == null)
        {
            throw new ArgumentNullException(nameof(user.Email));
        }

        if (await _authRepository.UserExists(user.Email))
        {
            throw new Exception("User already exists.");
        }

        return await _authRepository.Register(user);
    }

    public async Task<User> Login(string email, string password)
    {
        var user = await _authRepository.GetUserByEmail(email);
        if (user == null)
        {
            throw new Exception("User not found.");
        }
        if (user.Password == null || !VerifyPassword(password, user.Password))
        {
            throw new Exception("Incorrect password.");
        }
        return user;
    }
    public async Task<User> GetUserByEmail(string email)
    {
        return await _authRepository.GetUserByEmail(email);
    }

    public async Task<User> GetUserById(string id)
    {
        return await _authRepository.GetUserById(id);
    }

    public async Task<User> UpdateUser(User user)
    {
        return await _authRepository.UpdateUser(user);
    }

    public async Task<bool> DeleteUser(string id)
    {
        return await _authRepository.DeleteUser(id);
    }

    public async Task<bool> UserExists(string email)
    {
        return await _authRepository.UserExists(email);
    }


    public string GenerateJwtToken(User user)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var secretKey = _configuration["JwtSettings:SecretKey"];
        if (string.IsNullOrEmpty(secretKey))
        {
            throw new InvalidOperationException("JWT secret key is not configured.");
        }

        var key = Encoding.ASCII.GetBytes(secretKey);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
            new Claim(ClaimTypes.NameIdentifier, user.Id ?? string.Empty),
            user.Email != null ? new Claim(ClaimTypes.Email, user.Email) : new Claim(ClaimTypes.Email, string.Empty)
        }),
            Expires = DateTime.UtcNow.AddDays(7),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256)
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
    public string HashPassword(string password)
    {
        return BCrypt.Net.BCrypt.HashPassword(password);
    }
    public bool VerifyPassword(string password, string hash)
    {
        if (string.IsNullOrEmpty(hash))
        {
            return false;
        }
        return BCrypt.Net.BCrypt.Verify(password, hash);
    }

    public async Task<List<User>> GetUsersByIds(string[] userIds)
    {
        return await _authRepository.GetUsersByIds(userIds);
    }


}