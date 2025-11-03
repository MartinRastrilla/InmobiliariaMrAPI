using InmobiliariaMrAPI.Data;
using InmobiliariaMrAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace InmobiliariaMrAPI.Repositories;

public class ArchivoRepository : IArchivoRepository 
{
    private readonly AppDbContext _context;

    public ArchivoRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Archivo?> CreateArchivo(Archivo archivo)
    {
        _context.Archivos.Add(archivo);
         await _context.SaveChangesAsync();
        return archivo;
    }

    public async Task<bool> DeleteArchivo(int id)
    {
        var archivo = await _context.Archivos.FindAsync(id);
        if (archivo == null)
        {
            return false;
        }
        _context.Archivos.Remove(archivo);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<Archivo?> GetArchivoById(int id)
    {
        return await _context.Archivos.FindAsync(id);
    }

    public async Task<IEnumerable<Archivo>> GetArchivosByInmuebleId(int inmuebleId)
    {
        return await _context.Archivos
            .Include(a => a.ArchivoInmuebles)
            .Where(a => a.ArchivoInmuebles.Any(ai => ai.InmuebleId == inmuebleId))
            .ToListAsync();
    }
}