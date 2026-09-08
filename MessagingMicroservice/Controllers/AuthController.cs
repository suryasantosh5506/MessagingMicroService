using MessagingMicroservice.Application.Interfaces;
using MessagingMicroservice.Application.Models.AuthModels;
using Microsoft.AspNetCore.Mvc;

namespace MessagingMicroservice.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController:ControllerBase
{
    private readonly IAuthService _authService;
    
    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> RegisterAsync(RegisterRequest request)
    {
        await _authService.RegisterAsync(request);
        return Ok();
    }
    
    [HttpPost("login")]
    public async Task<IActionResult> LoginAsync(LoginRequest request)
    {
        var token=await _authService.LoginAsync(request);
        return Ok(token);
    }
}