using InmobiliariaMrAPI.Models.Inmueble;

namespace InmobiliariaMrAPI.Repositories;

public interface IPropietarioRepository
{
    Task<Propietario?> CreatePropietario(Propietario propietario);
    Task<bool> UpdatePropietario(Propietario propietario);
    Task<bool> DeletePropietario(int id);
    Task<Propietario?> GetPropietarioById(int id);
    Task<IEnumerable<Propietario>?> GetAllPropietarios();
    Task<Propietario?> GetPropietarioByUserId(int userId);
}