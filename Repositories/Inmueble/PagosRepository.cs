using InmobiliariaMrAPI.Data;
using InmobiliariaMrAPI.Models.Inmueble;
using Microsoft.EntityFrameworkCore;

namespace InmobiliariaMrAPI.Repositories;

public class PagosRepository : IPagosRepository
{
    private readonly AppDbContext _context;

    public PagosRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Pagos>?> GetPagosByContratoId(int contratoId)
    {
        return await _context.Pagos
            .Where(p => p.ContratoId == contratoId)
            .Include(p => p.Contrato)
            .Include(p => p.Inquilino)
            .OrderByDescending(p => p.PaymentDate)
            .ToListAsync();
    }

    public async Task<Pagos?> GetPagoById(int id)
    {
        return await _context.Pagos
            .Include(p => p.Contrato)
                .ThenInclude(c => c.Inmueble)
            .Include(p => p.Contrato)
                .ThenInclude(c => c.Propietario)
            .Include(p => p.Inquilino)
            .FirstOrDefaultAsync(p => p.Id == id);
    }
}

