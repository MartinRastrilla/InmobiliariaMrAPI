using InmobiliariaMrAPI.Data;
using InmobiliariaMrAPI.Models.Inmueble;
using Microsoft.EntityFrameworkCore;

namespace InmobiliariaMrAPI.Repositories;

public class InmuebleRepository : IInmuebleRepository
{
    private readonly AppDbContext _context;

    public InmuebleRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Inmueble>?> GetAllInmuebles()
    {
        return await _context.Inmuebles.ToListAsync();
    }
    
    public async Task<Inmueble?> GetInmuebleById(int id)
    {
        return await _context.Inmuebles.FindAsync(id);
    }

    public async Task<Inmueble?> GetInmuebleByTitle(string title)
    {
        return await _context.Inmuebles.FirstOrDefaultAsync(i => i.Title == title);
    }
    
    public async Task<Inmueble> CreateInmueble(Inmueble inmueble)
    {
        _context.Inmuebles.Add(inmueble);
        await _context.SaveChangesAsync();
        return inmueble;
    }

    public async Task<bool> UpdateInmueble(Inmueble inmueble)
    {
        _context.Inmuebles.Update(inmueble);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteInmueble(int id)
    {
        var inmueble = await GetInmuebleById(id);
        if (inmueble == null)
        {
            return false;
        }
        _context.Inmuebles.Remove(inmueble);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<IEnumerable<Inmueble>?> GetInmueblesByPropietarioId(int propietarioId)
    {
        return await _context.Inmuebles.Where(i => i.PropietarioId == propietarioId).ToListAsync();
    }

    public async Task<Inmueble?> GetInmuebleByTitleForPropietario(string title, int propietarioId)
    {
        return await _context.Inmuebles.FirstOrDefaultAsync(i => i.Title == title && i.PropietarioId == propietarioId);
    }
}