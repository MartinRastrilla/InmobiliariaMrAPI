using InmobiliariaMrAPI.Common;
using InmobiliariaMrAPI.DTOs;

namespace InmobiliariaMrAPI.Services;

public interface IPagoService
{
    Task<Result<PagoDto>> GetPagoById(int id, int userId);
    Task<Result<IEnumerable<PagoDto>>> GetPagosByContratoId(int contratoId, int userId);
}

