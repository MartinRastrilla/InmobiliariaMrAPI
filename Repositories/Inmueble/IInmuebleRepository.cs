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
    /* Task<IEnumerable<Inmueble>> GetInmueblesByContratoId(int contratoId);
    Task<IEnumerable<Inmueble>> GetInmueblesByInquilinoId(int inquilinoId);
    Task<IEnumerable<Inmueble>> GetInmueblesByArchivoId(int archivoId);
    Task<IEnumerable<Inmueble>> GetInmueblesByPagosId(int pagosId);
    Task<IEnumerable<Inmueble>> GetInmueblesByContratoInquilinoId(int contratoInquilinoId); */
}
