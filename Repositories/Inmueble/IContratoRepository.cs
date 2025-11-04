using InmobiliariaMrAPI.Models.Inmueble;

namespace InmobiliariaMrAPI.Repositories;

public interface IContratoRepository
{
    Task<Contrato?> GetContratoById(int id);
    Task<IEnumerable<Contrato>?> GetContratosByPropietarioId(int propietarioId);
}

