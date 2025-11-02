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

    [HttpGet]
    [Authorize]
    public async Task<IActionResult> GetAllInmueblesByUserId()
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userIdClaim == null)
        {
            return Unauthorized(new { Message = "Token inválido" });
        }

        int userId = int.Parse(userIdClaim);
        var result = await _inmuebleService.GetAllInmueblesByUserId(userId);
        
        if (!result.Success)
        {
            return NotFound(new { Message = result.ErrorMessage });
        }
        
        return Ok(result.Data);
    }

    [HttpGet("{id}")]
    [Authorize]
    public async Task<IActionResult> GetInmuebleById(int id)
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userIdClaim == null)
        {
            return Unauthorized(new { Message = "Token inválido" });
        }

        int userId = int.Parse(userIdClaim);
        var result = await _inmuebleService.GetInmuebleById(id, userId);
        
        if (!result.Success)
        {
            return NotFound(new { Message = result.ErrorMessage });
        }
        
        return Ok(result.Data);
    }

    [HttpGet("title/{title}")]
    [Authorize]
    public async Task<IActionResult> GetInmuebleByTitle(string title)
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userIdClaim == null)
        {
            return Unauthorized(new { Message = "Token inválido" });
        }

        int userId = int.Parse(userIdClaim);
        var result = await _inmuebleService.GetInmuebleByTitle(title, userId);
        
        if (!result.Success)
        {
            return NotFound(new { Message = result.ErrorMessage });
        }
        
        return Ok(result.Data);
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> CreateInmueble([FromBody] InmuebleDto inmuebleDto)
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userIdClaim == null)
        {
            return Unauthorized(new { Message = "Token inválido" });
        }

        int userId = int.Parse(userIdClaim);
        var result = await _inmuebleService.CreateInmuebleForUser(inmuebleDto, userId);

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
        if (id != inmuebleDto.Id)
        {
            return BadRequest(new { Message = "El ID no coincide" });
        }

        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userIdClaim == null)
        {
            return Unauthorized(new { Message = "Token inválido" });
        }

        int userId = int.Parse(userIdClaim);
        var result = await _inmuebleService.UpdateInmuebleForUser(inmuebleDto, userId);

        if (!result.Success)
        {
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
            return NotFound(new { Message = result.ErrorMessage });
        }

        return Ok(new { Message = "Inmueble eliminado correctamente" });
    }

    [HttpPut("disable-enable/{id}")]
    [Authorize]
    public async Task<IActionResult> DisableAndEnableInmueble(int id)
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userIdClaim == null)
        {
            return Unauthorized(new { Message = "Token inválido" });
        }

        int userId = int.Parse(userIdClaim);
        var result = await _inmuebleService.DisableAndEnableInmuebleForUser(id, userId);
        
        if (!result.Success)
        {
            return BadRequest(new { Message = result.ErrorMessage });
        }

        return Ok(new { Message = "Inmueble actualizado correctamente", Data = result.Data });
    }
}