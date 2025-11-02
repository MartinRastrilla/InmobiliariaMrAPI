using InmobiliariaMrAPI.Data;
using InmobiliariaMrAPI.Models.Inmueble;
using Microsoft.EntityFrameworkCore;

namespace InmobiliariaMrAPI.Repositories;

public class PropietarioRepository : IPropietarioRepository
{
    private readonly AppDbContext _context;

    public PropietarioRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Propietario>?> GetAllPropietarios()
    {
        return await _context.Propietarios.ToListAsync();
    }

    public async Task<Propietario?> GetPropietarioById(int id)
    {
        return await _context.Propietarios.FindAsync(id);
    }

    public async Task<Propietario?> GetPropietarioByUserId(int userId)
    {
        return await _context.Propietarios.FirstOrDefaultAsync(p => p.UserId == userId);
    }
    
    public async Task<Propietario?> CreatePropietario(Propietario propietario)
    {
        _context.Propietarios.Add(propietario);
        await _context.SaveChangesAsync();
        return propietario;
    }

    public async Task<bool> UpdatePropietario(Propietario propietario)
    {
        _context.Propietarios.Update(propietario);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeletePropietario(int id)
    {
        var propietario = await GetPropietarioById(id);
        if (propietario == null)
        {
            return false;
        }
        _context.Propietarios.Remove(propietario);
        await _context.SaveChangesAsync();
        return true;
    }
}