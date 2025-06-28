using AngularAuthAPI.Models.Dto;
using BOGOGMATCH_DOMAIN.MODELS.UserManagement;
using BOGOMATCH_DOMAIN.INTERFACE;
using BOGOMATCH_DOMAIN.MODELS.DTO;
using Google.Apis.Auth;
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
    public async Task<IActionResult> LoginAsync([FromBody] LoginRequestDto login)
    {
        var result = await _authService.LoginAsync(login.Email, login.Password);
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
        var result = await _authService.refreshTokenAsync(tokenDto);
        return Ok(result);
    }
    [AllowAnonymous]
    [HttpPost("Send-Reset-Email/{email}")]
    public async Task<IActionResult> SendResetEmail(string email)
    {
        var result = await _authService.SendResetEmailAsync(email);
        return Ok(new { Message = result });
    }
    [AllowAnonymous]
    [HttpPost("Reset-Password")]
    public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDTO dto)
    {
        var result = await _authService.ResetPasswordAsync(dto);
        return Ok(new { Message = result });
    }

    [HttpPost("signup-with-google")]
    [AllowAnonymous]
    public async Task<IActionResult> SignUpWithGoogle(string googleTokenRequest)
    {
        try
        {
            var payload = await GoogleJsonWebSignature.ValidateAsync(googleTokenRequest);
            string email = payload.Email;
            var user = new User
            {
                Email = email,
                FirstName = payload.Name,
                LastName = payload.Name,
                Password = null,
                IsGoogleAuthenticated = true,
                CreatedAt = DateTime.Now,
                Role = "Guest"
            };

            var tokenDto = await _authService.AthenticateViaGoogle(user);
            return Ok(tokenDto);
        }
        catch (InvalidJwtException)
        {
            return Unauthorized("Invalid Google token");
        }
    }

}
