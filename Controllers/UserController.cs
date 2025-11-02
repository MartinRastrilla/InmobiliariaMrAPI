using System.Security.Claims;
using System.Text.Json;
using System.Text.Json.Serialization;
using InmobiliariaMrAPI.DTOs;
using InmobiliariaMrAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InmobiliariaMrAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly IAuthService _authService;

    public UserController(IUserService userService, IAuthService authService)
    {
        _userService = userService;
        _authService = authService;
    }

    [HttpGet]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetAllUsers()
    {
        var users = await _userService.GetAllUsers();

        //? Serializar para evitar cycles errors
        var userSerialized = JsonSerializer.Serialize(users, new JsonSerializerOptions { ReferenceHandler = ReferenceHandler.IgnoreCycles });
        return Ok(userSerialized);
    }
    
    [HttpGet("{id}")]
    [Authorize]
    public async Task<IActionResult> GetUserById(int id)
    {
        var result = await _userService.GetUserById(id);
        
        if (!result.Success)
        {
            return NotFound(new { Message = result.ErrorMessage });
        }
        
        return Ok(result.Data);
    }

    [HttpGet("me")]
    [Authorize]
    public async Task<IActionResult> GetLoggedUser()
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userId == null)
        {
            return Unauthorized(new { Message = "Token inválido" });
        }
        
        var result = await _userService.GetUserById(int.Parse(userId));
        
        if (!result.Success)
        {
            return NotFound(new { Message = result.ErrorMessage });
        }
        
        return Ok(result.Data);
    }

    [HttpPut("me")]
    [Authorize]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> UpdateLoggedUser([FromForm] UserDto userDto)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userId == null)
        {
            return Unauthorized(new { Message = "Token inválido" });
        }

        //? Obtener los roles
        var roles = User.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value).ToList();

        userDto.Roles = roles;
        foreach (var role in userDto.Roles)
        {
            Console.WriteLine(role);
        }

        var result = await _userService.UpdateLoggedUser(int.Parse(userId), userDto, userDto.ProfilePic);
        
        if (!result.Success)
        {
            return BadRequest(new { Message = result.ErrorMessage });
        }
        
        return Ok(new { Message = "Usuario actualizado correctamente", Data = result.Data });
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteUser(int id)
    {
        var result = await _userService.DeleteUser(id);
        
        if (!result.Success)
        {
            return NotFound(new { Message = result.ErrorMessage });
        }
        
        return Ok(new { Message = "Usuario eliminado correctamente" });
    }

    [HttpPut("me/password")]
    [Authorize]
    public async Task<IActionResult> UpdatePassword([FromBody] UserUpdatePasswordDto userUpdatePasswordDto)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userId == null)
        {
            return Unauthorized(new { Message = "Usuario no autenticado" });
        }

        //? Validar que la nueva contraseña sea diferente a la actual
        var validation = _authService.ValidatePassword(userUpdatePasswordDto.NewPassword);
        if (validation != "Password is valid")
        {
            return BadRequest(new { Message = validation });
        }

        var result = await _userService.UpdatePassword(int.Parse(userId), userUpdatePasswordDto);
        if (!result.Success)
        {
            return BadRequest(new { Message = result.ErrorMessage });
        }

        return Ok(new { Message = "Contraseña actualizada correctamente", Data = result.Data });
    }
}