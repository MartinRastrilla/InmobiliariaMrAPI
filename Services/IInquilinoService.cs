using InmobiliariaMrAPI.Common;
using InmobiliariaMrAPI.DTOs;

namespace InmobiliariaMrAPI.Services;

public interface IInquilinoService
{
    Task<Result<InquilinoDto>> GetInquilinoById(int id, int userId);
}

