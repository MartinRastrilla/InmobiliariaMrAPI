using InmobiliariaMrAPI.DTOs;
using InmobiliariaMrAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace InmobiliariaMrAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] UserLoginDto userLoginDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var result = await _authService.Login(userLoginDto.Email, userLoginDto.Password);
        
        if (!result.Success)
        {
            return Unauthorized(new { Message = result.ErrorMessage });
        }
        
        return Ok(new { Token = result.Data });
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] UserRegisterDto userDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var result = await _authService.Register(userDto);
        
        if (!result.Success)
        {
            return BadRequest(new { Message = result.ErrorMessage });
        }
        
        return Ok(new { Message = "Usuario registrado exitosamente", UserId = result.Data });
    }

    [HttpPost("logout")]
    public IActionResult Logout()
    {
        return Ok(new { Message = "Sesión cerrada exitosamente" });
    }
}