using InmobiliariaMrAPI.Common;
using InmobiliariaMrAPI.DTOs;
using InmobiliariaMrAPI.Models.Inmueble;
using InmobiliariaMrAPI.Repositories;

namespace InmobiliariaMrAPI.Services;

public class ContratoService : IContratoService
{
    private readonly IContratoRepository _contratoRepository;
    private readonly IPropietarioRepository _propietarioRepository;

    public ContratoService(IContratoRepository contratoRepository, IPropietarioRepository propietarioRepository)
    {
        _contratoRepository = contratoRepository;
        _propietarioRepository = propietarioRepository;
    }

    public async Task<Result<IEnumerable<ContratoDto>>> GetContratosByUserId(int userId)
    {
        //? Obtener propietario desde userId
        var propietario = await _propietarioRepository.GetPropietarioByUserId(userId);
        if (propietario == null)
        {
            return Result<IEnumerable<ContratoDto>>.Fail("Propietario no encontrado");
        }

        //? Obtener contratos del propietario
        var contratos = await _contratoRepository.GetContratosByPropietarioId(propietario.Id);
        if (contratos == null || !contratos.Any())
        {
            return Result<IEnumerable<ContratoDto>>.Fail("No se encontraron contratos para este propietario");
        }

        //? Mapear a DTOs
        var contratosDto = contratos.Select(c => MapContratoToDto(c)).ToList();
        
        return Result<IEnumerable<ContratoDto>>.Ok(contratosDto);
    }

    public async Task<Result<ContratoDto>> GetContratoById(int id, int userId)
    {
        //? Obtener propietario desde userId
        var propietario = await _propietarioRepository.GetPropietarioByUserId(userId);
        if (propietario == null)
        {
            return Result<ContratoDto>.Fail("Propietario no encontrado");
        }

        //? Obtener contrato
        var contrato = await _contratoRepository.GetContratoById(id);
        if (contrato == null)
        {
            return Result<ContratoDto>.Fail("Contrato no encontrado");
        }

        //? Verificar que el contrato pertenece al propietario
        if (contrato.PropietarioId != propietario.Id)
        {
            return Result<ContratoDto>.Fail("No tienes permisos para ver este contrato");
        }

        //? Mapear a DTO
        var contratoDto = MapContratoToDto(contrato);
        
        return Result<ContratoDto>.Ok(contratoDto);
    }

    private ContratoDto MapContratoToDto(Contrato contrato)
    {
        return new ContratoDto
        {
            Id = contrato.Id,
            StartDate = contrato.StartDate,
            EndDate = contrato.EndDate,
            TotalPrice = contrato.TotalPrice,
            MonthlyPrice = contrato.MonthlyPrice,
            Status = contrato.Status.ToString(),
            CreatedAt = contrato.CreatedAt,
            UpdatedAt = contrato.UpdatedAt,
            Inmueble = new InmuebleInfoDto
            {
                Id = contrato.Inmueble.Id,
                Title = contrato.Inmueble.Title,
                Address = contrato.Inmueble.Address,
                Latitude = contrato.Inmueble.Latitude,
                Longitude = contrato.Inmueble.Longitude,
                Rooms = contrato.Inmueble.Rooms,
                Price = contrato.Inmueble.Price,
                MaxGuests = contrato.Inmueble.MaxGuests,
                Available = contrato.Inmueble.Available
            },
            Inquilinos = contrato.ContratoInquilinos.Select(ci => new InquilinoInfoDto
            {
                Id = ci.Inquilino.Id,
                Name = ci.Inquilino.Name,
                LastName = ci.Inquilino.LastName,
                DocumentNumber = ci.Inquilino.DocumentNumber,
                Phone = ci.Inquilino.Phone,
                Email = ci.Inquilino.Email,
                IsPaymentResponsible = ci.IsPaymentResponsible
            }).ToList()
        };
    }
}

