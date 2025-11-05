using InmobiliariaMrAPI.Common;
using InmobiliariaMrAPI.DTOs;
using InmobiliariaMrAPI.Models.Inmueble;
using InmobiliariaMrAPI.Repositories;

namespace InmobiliariaMrAPI.Services;

public class PagoService : IPagoService
{
    private readonly IPagosRepository _pagosRepository;
    private readonly IPropietarioRepository _propietarioRepository;
    private readonly IContratoRepository _contratoRepository;

    public PagoService(IPagosRepository pagosRepository, IPropietarioRepository propietarioRepository, IContratoRepository contratoRepository)
    {
        _pagosRepository = pagosRepository;
        _propietarioRepository = propietarioRepository;
        _contratoRepository = contratoRepository;
    }

    public async Task<Result<PagoDto>> GetPagoById(int id, int userId)
    {
        //? Obtener propietario desde userId
        var propietario = await _propietarioRepository.GetPropietarioByUserId(userId);
        if (propietario == null)
        {
            return Result<PagoDto>.Fail("Propietario no encontrado");
        }

        //? Obtener pago
        var pago = await _pagosRepository.GetPagoById(id);
        if (pago == null)
        {
            return Result<PagoDto>.Fail("Pago no encontrado");
        }

        //? Verificar que el contrato del pago pertenece al propietario autenticado
        if (pago.Contrato.PropietarioId != propietario.Id)
        {
            return Result<PagoDto>.Fail("No tienes permisos para ver este pago");
        }

        //? Mapear a DTO
        var pagoDto = MapPagoToDto(pago);
        
        return Result<PagoDto>.Ok(pagoDto);
    }

    public async Task<Result<IEnumerable<PagoDto>>> GetPagosByContratoId(int contratoId, int userId)
    {
        //? Obtener propietario desde userId
        var propietario = await _propietarioRepository.GetPropietarioByUserId(userId);
        if (propietario == null)
        {
            return Result<IEnumerable<PagoDto>>.Fail("Propietario no encontrado");
        }

        //? Verificar que el contrato existe
        var contrato = await _contratoRepository.GetContratoById(contratoId);
        if (contrato == null)
        {
            return Result<IEnumerable<PagoDto>>.Fail("Contrato no encontrado");
        }

        //? Verificar que el contrato pertenece al propietario autenticado
        if (contrato.PropietarioId != propietario.Id)
        {
            return Result<IEnumerable<PagoDto>>.Fail("No tienes permisos para ver los pagos de este contrato");
        }

        //? Obtener pagos del contrato
        var pagos = await _pagosRepository.GetPagosByContratoId(contratoId);
        if (pagos == null || !pagos.Any())
        {
            return Result<IEnumerable<PagoDto>>.Fail("No se encontraron pagos para este contrato");
        }

        //? Mapear a DTOs
        var pagosDto = pagos.Select(p => MapPagoToDto(p)).ToList();
        
        return Result<IEnumerable<PagoDto>>.Ok(pagosDto);
    }

    private PagoDto MapPagoToDto(Pagos pago)
    {
        return new PagoDto
        {
            Id = pago.Id,
            Amount = pago.Amount,
            PaymentDate = pago.PaymentDate,
            PaymentMethod = pago.PaymentMethod.ToString(),
            CreatedAt = pago.CreatedAt,
            UpdatedAt = pago.UpdatedAt,
            Contrato = new ContratoPagoInfoDto
            {
                Id = pago.Contrato.Id,
                StartDate = pago.Contrato.StartDate,
                EndDate = pago.Contrato.EndDate,
                TotalPrice = pago.Contrato.TotalPrice,
                MonthlyPrice = pago.Contrato.MonthlyPrice,
                Status = pago.Contrato.Status.ToString(),
                Inmueble = new InmuebleInfoDto
                {
                    Id = pago.Contrato.Inmueble.Id,
                    Title = pago.Contrato.Inmueble.Title,
                    Address = pago.Contrato.Inmueble.Address,
                    Latitude = pago.Contrato.Inmueble.Latitude,
                    Longitude = pago.Contrato.Inmueble.Longitude,
                    Rooms = pago.Contrato.Inmueble.Rooms,
                    Price = pago.Contrato.Inmueble.Price,
                    MaxGuests = pago.Contrato.Inmueble.MaxGuests,
                    Available = pago.Contrato.Inmueble.Available
                }
            },
            Inquilino = new InquilinoPagoInfoDto
            {
                Id = pago.Inquilino.Id,
                Name = pago.Inquilino.Name,
                LastName = pago.Inquilino.LastName,
                DocumentNumber = pago.Inquilino.DocumentNumber,
                Phone = pago.Inquilino.Phone,
                Email = pago.Inquilino.Email
            }
        };
    }
}

