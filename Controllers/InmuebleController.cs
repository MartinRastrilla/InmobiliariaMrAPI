using System.Security.Claims;
using InmobiliariaMrAPI.DTOs;
using InmobiliariaMrAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InmobiliariaMrAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class InmuebleController : ControllerBase
{
    private readonly IInmuebleService _inmuebleService;

    public InmuebleController(IInmuebleService inmuebleService)
    {
        _inmuebleService = inmuebleService;
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
    [Authorize]
    public async Task<IActionResult> GetAllInmueblesByUserId()
    {
        var userId = GetUserId();
        if (userId == null)
        {
            return Unauthorized(new { Message = "Token inválido o usuario no autenticado" });
        }

        var result = await _inmuebleService.GetAllInmueblesByUserId(userId.Value);
        
        if (!result.Success)
        {
            // Si no hay inmuebles, es válido devolver lista vacía, no un error
            if (result.ErrorMessage?.Contains("no encontrado") == true)
            {
                return NotFound(new { Message = result.ErrorMessage });
            }
            return BadRequest(new { Message = result.ErrorMessage });
        }
        
        return Ok(result.Data);
    }

    [HttpGet("{id}")]
    [Authorize]
    public async Task<IActionResult> GetInmuebleById(int id)
    {
        var userId = GetUserId();
        if (userId == null)
        {
            return Unauthorized(new { Message = "Token inválido o usuario no autenticado" });
        }

        var result = await _inmuebleService.GetInmuebleById(id, userId.Value);
        
        if (!result.Success)
        {
            if (result.ErrorMessage?.Contains("no encontrado") == true || 
                result.ErrorMessage?.Contains("permisos") == true)
            {
                return NotFound(new { Message = result.ErrorMessage });
            }
            return BadRequest(new { Message = result.ErrorMessage });
        }
        
        return Ok(result.Data);
    }

    [HttpGet("title/{title}")]
    [Authorize]
    public async Task<IActionResult> GetInmuebleByTitle(string title)
    {
        if (string.IsNullOrWhiteSpace(title))
        {
            return BadRequest(new { Message = "El título es requerido" });
        }

        var userId = GetUserId();
        if (userId == null)
        {
            return Unauthorized(new { Message = "Token inválido o usuario no autenticado" });
        }

        var result = await _inmuebleService.GetInmuebleByTitle(title, userId.Value);
        
        if (!result.Success)
        {
            if (result.ErrorMessage?.Contains("no encontrado") == true || 
                result.ErrorMessage?.Contains("permisos") == true)
            {
                return NotFound(new { Message = result.ErrorMessage });
            }
            return BadRequest(new { Message = result.ErrorMessage });
        }
        
        return Ok(result.Data);
    }

    [HttpPost]
    [Authorize]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> CreateInmueble([FromForm] InmuebleDto inmuebleDto)
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

        var result = await _inmuebleService.CreateInmuebleForUser(inmuebleDto, userId.Value);

        if (!result.Success)
        {
            return BadRequest(new { Message = result.ErrorMessage });
        }

        return CreatedAtAction(nameof(GetInmuebleById), new { id = result.Data!.Id }, result.Data);
    }

    [HttpPut("{id}")]
    [Authorize]
    public async Task<IActionResult> UpdateInmueble(int id, [FromBody] InmuebleDto inmuebleDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        if (id != inmuebleDto.Id)
        {
            return BadRequest(new { Message = "El ID de la URL no coincide con el ID del cuerpo de la petición" });
        }

        var userId = GetUserId();
        if (userId == null)
        {
            return Unauthorized(new { Message = "Token inválido o usuario no autenticado" });
        }

        var result = await _inmuebleService.UpdateInmuebleForUser(inmuebleDto, userId.Value);

        if (!result.Success)
        {
            if (result.ErrorMessage?.Contains("no encontrado") == true || 
                result.ErrorMessage?.Contains("permisos") == true)
            {
                return NotFound(new { Message = result.ErrorMessage });
            }
            return BadRequest(new { Message = result.ErrorMessage });
        }

        return Ok(new { Message = "Inmueble actualizado correctamente", Data = result.Data });
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteInmueble(int id)
    {
        var result = await _inmuebleService.DeleteInmueble(id);

        if (!result.Success)
        {
            if (result.ErrorMessage?.Contains("no encontrado") == true)
            {
                return NotFound(new { Message = result.ErrorMessage });
            }
            return BadRequest(new { Message = result.ErrorMessage });
        }

        return Ok(new { Message = "Inmueble eliminado correctamente" });
    }

    [HttpPut("disable-enable/{id}")]
    [Authorize]
    public async Task<IActionResult> DisableAndEnableInmueble(int id)
    {
        var userId = GetUserId();
        if (userId == null)
        {
            return Unauthorized(new { Message = "Token inválido o usuario no autenticado" });
        }

        var result = await _inmuebleService.DisableAndEnableInmuebleForUser(id, userId.Value);
        
        if (!result.Success)
        {
            if (result.ErrorMessage?.Contains("no encontrado") == true || 
                result.ErrorMessage?.Contains("permisos") == true)
            {
                return NotFound(new { Message = result.ErrorMessage });
            }
            return BadRequest(new { Message = result.ErrorMessage });
        }

        return Ok(new { Message = "Inmueble actualizado correctamente", Data = result.Data });
    }
}