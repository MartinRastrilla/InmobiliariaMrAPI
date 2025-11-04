using InmobiliariaMrAPI.Data;
using InmobiliariaMrAPI.Models.Inmueble;
using Microsoft.EntityFrameworkCore;

namespace InmobiliariaMrAPI.Repositories;

public class ContratoRepository : IContratoRepository
{
    private readonly AppDbContext _context;

    public ContratoRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Contrato?> GetContratoById(int id)
    {
        return await _context.Contratos
            .Include(c => c.Inmueble)
            .Include(c => c.Propietario)
            .Include(c => c.ContratoInquilinos)
                .ThenInclude(ci => ci.Inquilino)
            .FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task<IEnumerable<Contrato>?> GetContratosByPropietarioId(int propietarioId)
    {
        return await _context.Contratos
            .Where(c => c.PropietarioId == propietarioId)
            .Include(c => c.Inmueble)
            .Include(c => c.Propietario)
            .Include(c => c.ContratoInquilinos)
                .ThenInclude(ci => ci.Inquilino)
            .OrderByDescending(c => c.CreatedAt)
            .ToListAsync();
    }
}

