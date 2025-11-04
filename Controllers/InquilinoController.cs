using System.Security.Claims;
using InmobiliariaMrAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InmobiliariaMrAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class InquilinoController : ControllerBase
{
    private readonly IInquilinoService _inquilinoService;

    public InquilinoController(IInquilinoService inquilinoService)
    {
        _inquilinoService = inquilinoService;
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

    [HttpGet("{id}")]
    [Authorize]
    public async Task<IActionResult> GetInquilinoById(int id)
    {
        var userId = GetUserId();
        if (userId == null)
        {
            return Unauthorized(new { Message = "Token inválido o usuario no autenticado" });
        }

        var result = await _inquilinoService.GetInquilinoById(id, userId.Value);
        
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
}

