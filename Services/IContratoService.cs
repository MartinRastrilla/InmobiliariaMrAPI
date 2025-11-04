using InmobiliariaMrAPI.Common;
using InmobiliariaMrAPI.DTOs;

namespace InmobiliariaMrAPI.Services;

public interface IContratoService
{
    Task<Result<IEnumerable<ContratoDto>>> GetContratosByUserId(int userId);
    Task<Result<ContratoDto>> GetContratoById(int id, int userId);
}

