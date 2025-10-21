using InmobiliariaMrAPI.Common;
using InmobiliariaMrAPI.DTOs;

namespace InmobiliariaMrAPI.Services;

public interface IInmuebleService
{
    Task<IEnumerable<InmuebleDto>> GetAllInmuebles();
    Task<Result<InmuebleDto>> GetInmuebleById(int id);
    Task<Result<InmuebleDto>> GetInmuebleByTitle(string title);
    Task<Result<InmuebleDto>> CreateInmuebleForUser(InmuebleDto inmuebleDto, int userId);
    Task<Result<InmuebleDto>> UpdateInmuebleForUser(InmuebleDto inmuebleDto, int userId);
    Task<Result<bool>> DeleteInmueble(int id);
    Task<IEnumerable<InmuebleDto>> GetInmueblesByPropietarioId(int propietarioId);
}