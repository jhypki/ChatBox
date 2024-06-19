using Models;
using Services;
using Microsoft.AspNetCore.Mvc;

namespace Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(User user)
    {
        try
        {
            if (user.Password != null)
            {
                user.Password = _authService.HashPassword(user.Password);
            }
            var registeredUser = await _authService.Register(user);
            string token = _authService.GenerateJwtToken(registeredUser);
            return Ok(new { user = registeredUser, token });
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginModel loginModel)
    {
        try
        {
            var user = await _authService.Login(loginModel.Email, loginModel.Password);
            string token = _authService.GenerateJwtToken(user);
            string hashPassword = _authService.HashPassword(loginModel.Password);

            return Ok(new { user.Email, user.Username, token });
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
    //TODO move to user controller
    // [HttpGet("get/{email}")]
    // public async Task<IActionResult> GetUserByEmail(string email)
    // {
    //     try
    //     {
    //         var user = await _authService.GetUserByEmail(email);
    //         return Ok(user);
    //     }
    //     catch (Exception ex)
    //     {
    //         return BadRequest(ex.Message);
    //     }
    // }

    // [HttpGet("get-user-by-id")]
    // public async Task<IActionResult> GetUserById(string id)
    // {
    //     try
    //     {
    //         var user = await _authService.GetUserById(id);
    //         return Ok(user);
    //     }
    //     catch (Exception ex)
    //     {
    //         return BadRequest(ex.Message);
    //     }
    // }

    // [HttpPut("update-user")]
    // public async Task<IActionResult> UpdateUser(User user)
    // {
    //     try
    //     {
    //         var updatedUser = await _authService.UpdateUser(user);
    //         return Ok(updatedUser);
    //     }
    //     catch (Exception ex)
    //     {
    //         return BadRequest(ex.Message);
    //     }
    // }

    // [HttpDelete("delete-user")]
    // public async Task<IActionResult> DeleteUser(string id)
    // {
    //     try
    //     {
    //         var result = await _authService.DeleteUser(id);
    //         return Ok(result);
    //     }
    //     catch (Exception ex)
    //     {
    //         return BadRequest(ex.Message);
    //     }
    // }
}

