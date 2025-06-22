using AngularAuthAPI.Models.Dto;
using BOGOGMATCH_DOMAIN.MODELS.UserManagement;
using BOGOMATCH_DOMAIN.INTERFACE;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;


[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{

    private readonly IUserAuthService _authService;

    public UserController(IUserAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("Login")]
    [AllowAnonymous]
    public async Task<IActionResult> LoginAsync(string email, string password)
    {
        var result = await _authService.LoginAsync(email, password);
        return Ok(result);
    }

    [HttpPost("Register")]
    [AllowAnonymous]
    public async Task<IActionResult> Register([FromBody] User userObj)
    {
        var result = await _authService.RegisterUserAsync(userObj);
        return Ok(new { Message = result });
    }

    [Authorize(Roles = "Admin")]
    [HttpGet]
    public async Task<IActionResult> GetAllUsers()
    {
        var users = await _authService.GetAllUsersAsync();
        if (!users.Any())
        {
            return NotFound("No users found.");
        }
        return Ok(users);
    }

    [HttpPost("Refresh")]
    public async Task<IActionResult> Refresh([FromBody] TokenApiDTO tokenDto)
    {
        var result = await _authService.RefreshTokenAsync(tokenDto);
        return Ok(result);
    }

    [HttpPost("Send-Reset-Email/{email}")]
    public async Task<IActionResult> SendResetEmail(string email)
    {
        var result = await _authService.SendResetEmailAsync(email);
        return Ok(new { Message = result });
    }

    [HttpPost("Reset-Password")]
    public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDTO dto)
    {
        var result = await _authService.ResetPasswordAsync(dto);
        return Ok(new { Message = result });
    }
}
