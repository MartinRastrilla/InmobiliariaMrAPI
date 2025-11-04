using InmobiliariaMrAPI.Models.Inmueble;

namespace InmobiliariaMrAPI.Repositories;

public interface IInquilinoRepository
{
    Task<Inquilino?> GetInquilinoById(int id);
}

