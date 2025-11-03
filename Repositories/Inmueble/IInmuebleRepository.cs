using InmobiliariaMrAPI.Models.Inmueble;

namespace InmobiliariaMrAPI.Repositories;

public interface IInmuebleRepository
{
    //? ================================= MÉTODOS BÁSICOS =================================
    Task<IEnumerable<Inmueble>?> GetAllInmuebles();
    Task<Inmueble?> GetInmuebleById(int id);
    Task<Inmueble?> GetInmuebleByTitle(string title);
    Task<Inmueble> CreateInmueble(Inmueble inmueble);
    Task<bool> UpdateInmueble(Inmueble inmueble);
    Task<bool> DeleteInmueble(int id);

    //? ================================= MÉTODOS ESPECÍFICOS =================================
    Task<IEnumerable<Inmueble>?> GetInmueblesByPropietarioId(int propietarioId);
    Task<Inmueble?> GetInmuebleByTitleForPropietario(string title, int propietarioId);
    Task<bool> LinkArchivosToInmueble(int inmuebleId, List<int> archivoIds);
    Task<bool> LinkArchivoToInmueble(int inmuebleId, int archivoId);
}
