using InmobiliariaMrAPI.Data;
using Microsoft.EntityFrameworkCore;

namespace InmobiliariaMrAPI.Repositories.User;

public class UserRepository : IUserRepository
{
    private readonly AppDbContext _context;

    public UserRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Models.User.User>> GetAllUsers()
    {
        return await _context.Users
            .Include(u => u.UserRoles)
                .ThenInclude(ur => ur.Role)
            .ToListAsync();
    }

    public async Task<Models.User.User?> GetUserById(int id)
    {
        return await _context.Users
            .Include(u => u.UserRoles)
                .ThenInclude(ur => ur.Role)
            .FirstOrDefaultAsync(u => u.Id == id);
    }

    public async Task<Models.User.User?> GetUserByEmail(string email)
    {
        return await _context.Users
            .Include(u => u.UserRoles)
                .ThenInclude(ur => ur.Role)
            .FirstOrDefaultAsync(u => u.Email == email);
    }
    
    public async Task<Models.User.User> CreateUser(Models.User.User user)
    {
        using var transaction = await _context.Database.BeginTransactionAsync();
        try
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            await transaction.CommitAsync();
            return user;
        }
        catch
        {
            await transaction.RollbackAsync();
            throw new Exception("Error creating user");
        }
    }
    
    public async Task<bool> UpdateUser(Models.User.User user)
    {
        _context.Users.Update(user);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteUser(int id)
    {
        var user = await GetUserById(id);
        if (user == null)
        {
            return false;
        }
        _context.Users.Remove(user);
        await _context.SaveChangesAsync();
        return true;
    }
}