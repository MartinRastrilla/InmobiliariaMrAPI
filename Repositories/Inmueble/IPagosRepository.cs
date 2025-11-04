using InmobiliariaMrAPI.Models.Inmueble;

namespace InmobiliariaMrAPI.Repositories;

public interface IPagosRepository
{
    Task<IEnumerable<Pagos>?> GetPagosByContratoId(int contratoId);
    Task<Pagos?> GetPagoById(int id);
}

