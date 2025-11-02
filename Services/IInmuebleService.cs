using InmobiliariaMrAPI.Common;
using InmobiliariaMrAPI.DTOs;

namespace InmobiliariaMrAPI.Services;

public interface IInmuebleService
{
    Task<Result<IEnumerable<InmuebleDto>>> GetAllInmueblesByUserId(int userId);
    Task<Result<InmuebleDto>> GetInmuebleById(int id, int userId);
    Task<Result<InmuebleDto>> GetInmuebleByTitle(string title, int userId);
    Task<Result<InmuebleDto>> CreateInmuebleForUser(InmuebleDto inmuebleDto, int userId);
    Task<Result<InmuebleDto>> UpdateInmuebleForUser(InmuebleDto inmuebleDto, int userId);
    Task<Result<bool>> DisableAndEnableInmuebleForUser(int id, int userId);
    Task<Result<bool>> DeleteInmueble(int id);
}