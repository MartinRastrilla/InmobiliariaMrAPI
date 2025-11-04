using System.Security.Claims;
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

    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    //? Extrae el ID del usuario autenticado desde los claims
    private int? GetUserId()
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int userId))
        {
            return null;
        }
        return userId;
    }

    [HttpGet]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetAllUsers()
    {
        var result = await _userService.GetAllUsers();
        
        if (!result.Success)
        {
            return BadRequest(new { Message = result.ErrorMessage });
        }
        
        return Ok(result.Data);
    }
    
    [HttpGet("{id}")]
    [Authorize]
    public async Task<IActionResult> GetUserById(int id)
    {
        var result = await _userService.GetUserById(id);
        
        if (!result.Success)
        {
            if (result.ErrorMessage?.Contains("no encontrado") == true)
            {
                return NotFound(new { Message = result.ErrorMessage });
            }
            return BadRequest(new { Message = result.ErrorMessage });
        }
        
        return Ok(result.Data);
    }

    [HttpGet("me")]
    [Authorize]
    public async Task<IActionResult> GetLoggedUser()
    {
        var userId = GetUserId();
        if (userId == null)
        {
            return Unauthorized(new { Message = "Token inválido o usuario no autenticado" });
        }
        
        var result = await _userService.GetUserById(userId.Value);
        
        if (!result.Success)
        {
            if (result.ErrorMessage?.Contains("no encontrado") == true)
            {
                return NotFound(new { Message = result.ErrorMessage });
            }
            return BadRequest(new { Message = result.ErrorMessage });
        }
        
        return Ok(result.Data);
    }

    [HttpPut("me")]
    [Authorize]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> UpdateLoggedUser([FromForm] UserDto userDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var userId = GetUserId();
        if (userId == null)
        {
            return Unauthorized(new { Message = "Token inválido o usuario no autenticado" });
        }

        //? Obtener los roles
        var roles = User.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value).ToList();
        userDto.Roles = roles;

        var result = await _userService.UpdateLoggedUser(userId.Value, userDto, userDto.ProfilePic);
        
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
            if (result.ErrorMessage?.Contains("no encontrado") == true)
            {
                return NotFound(new { Message = result.ErrorMessage });
            }
            return BadRequest(new { Message = result.ErrorMessage });
        }
        
        return Ok(new { Message = "Usuario eliminado correctamente" });
    }

    [HttpPut("me/password")]
    [Authorize]
    public async Task<IActionResult> UpdatePassword([FromBody] UserUpdatePasswordDto userUpdatePasswordDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var userId = GetUserId();
        if (userId == null)
        {
            return Unauthorized(new { Message = "Token inválido o usuario no autenticado" });
        }

        var result = await _userService.UpdatePassword(userId.Value, userUpdatePasswordDto);
        
        if (!result.Success)
        {
            return BadRequest(new { Message = result.ErrorMessage });
        }

        return Ok(new { Message = "Contraseña actualizada correctamente", Data = result.Data });
    }
}