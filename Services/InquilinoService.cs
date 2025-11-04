using InmobiliariaMrAPI.Common;
using InmobiliariaMrAPI.DTOs;
using InmobiliariaMrAPI.Models.Inmueble;
using InmobiliariaMrAPI.Repositories;

namespace InmobiliariaMrAPI.Services;

public class InquilinoService : IInquilinoService
{
    private readonly IInquilinoRepository _inquilinoRepository;
    private readonly IPropietarioRepository _propietarioRepository;

    public InquilinoService(IInquilinoRepository inquilinoRepository, IPropietarioRepository propietarioRepository)
    {
        _inquilinoRepository = inquilinoRepository;
        _propietarioRepository = propietarioRepository;
    }

    public async Task<Result<InquilinoDto>> GetInquilinoById(int id, int userId)
    {
        //? Obtener propietario desde userId
        var propietario = await _propietarioRepository.GetPropietarioByUserId(userId);
        if (propietario == null)
        {
            return Result<InquilinoDto>.Fail("Propietario no encontrado");
        }

        //? Obtener inquilino
        var inquilino = await _inquilinoRepository.GetInquilinoById(id);
        if (inquilino == null)
        {
            return Result<InquilinoDto>.Fail("Inquilino no encontrado");
        }

        //? Verificar que el inquilino tenga contratos con el propietario autenticado
        var contratosDelPropietario = inquilino.ContratoInquilinos
            .Where(ci => ci.Contrato.PropietarioId == propietario.Id && ci.InquilinoId == inquilino.Id)
            .ToList();

        Console.WriteLine(contratosDelPropietario.Count());

        if (!contratosDelPropietario.Any())
        {
            return Result<InquilinoDto>.Fail("No tienes permisos para ver este inquilino");
        }

        //? Mapear a DTO
        var inquilinoDto = MapInquilinoToDto(inquilino, contratosDelPropietario);
        
        return Result<InquilinoDto>.Ok(inquilinoDto);
    }

    private InquilinoDto MapInquilinoToDto(Inquilino inquilino, List<ContratoInquilino> contratosDelPropietario)
    {
        return new InquilinoDto
        {
            Id = inquilino.Id,
            Name = inquilino.Name,
            LastName = inquilino.LastName,
            DocumentNumber = inquilino.DocumentNumber,
            Phone = inquilino.Phone,
            Email = inquilino.Email,
            IsActive = inquilino.IsActive,
            CreatedAt = inquilino.CreatedAt,
            UpdatedAt = inquilino.UpdatedAt
        };
    }
}

