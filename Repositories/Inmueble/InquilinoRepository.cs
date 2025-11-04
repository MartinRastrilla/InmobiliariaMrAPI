using InmobiliariaMrAPI.Data;
using InmobiliariaMrAPI.Models.Inmueble;
using Microsoft.EntityFrameworkCore;

namespace InmobiliariaMrAPI.Repositories;

public class InquilinoRepository : IInquilinoRepository
{
    private readonly AppDbContext _context;

    public InquilinoRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Inquilino?> GetInquilinoById(int id)
    {
        return await _context.Inquilinos
            .Include(i => i.ContratoInquilinos)
                .ThenInclude(ci => ci.Contrato)
            .FirstOrDefaultAsync(i => i.Id == id);
    }
}

