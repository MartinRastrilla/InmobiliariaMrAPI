using InmobiliariaMrAPI.Models;

namespace InmobiliariaMrAPI.Repositories;

public interface IArchivoRepository
{
    Task<Archivo?> CreateArchivo(Archivo archivo);
    Task<bool> DeleteArchivo(int id);
    Task<Archivo?> GetArchivoById(int id);
    Task<IEnumerable<Archivo>> GetArchivosByInmuebleId(int inmuebleId);
}