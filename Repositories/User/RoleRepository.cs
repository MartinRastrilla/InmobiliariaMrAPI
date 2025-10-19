using InmobiliariaMrAPI.Data;
using InmobiliariaMrAPI.Models.User;
using Microsoft.EntityFrameworkCore;

namespace InmobiliariaMrAPI.Repositories.User;

public class RoleRepository : IRoleRepository
{
    private readonly AppDbContext _context;

    public RoleRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Role>> GetAllRoles()
    {
        return await _context.Roles.ToListAsync() ?? throw new Exception("No roles found");
    }

    public async Task<Role> GetRoleById(int id)
    {
        return await _context.Roles.FindAsync(id) ?? throw new Exception("Role not found");
    }

    public async Task<Role> GetRoleByName(string name)
    {
        return await _context.Roles.FirstOrDefaultAsync(r => r.Name == name) ?? throw new Exception("Role not found");
    }
    
    public async Task<Role> CreateRole(Role role)
    {
        _context.Roles.Add(role);
        await _context.SaveChangesAsync();
        return role;
    }

    public async Task<bool> UpdateRole(Role role)
    {
        _context.Roles.Update(role);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteRole(int id)
    {
        var role = await GetRoleById(id);
        _context.Roles.Remove(role);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<IEnumerable<Role>> GetRolesByUserId(int userId)
    {
        var roles = await _context.UserRoles.Where(ur => ur.UserId == userId).Select(ur => ur.Role).ToListAsync();
        if (roles == null)
        {
            return [];
        }
        return roles;
    }

    public async Task<bool> AddRolesToUser(int userId, List<string> roles)
    {
        var userRoles = new List<UserRole>();
        foreach (var role in roles)
        {
            var roleEntity = await GetRoleByName(role);
            if (roleEntity == null) return false;
            userRoles.Add(new UserRole { UserId = userId, RoleId = roleEntity.Id });
        }
        _context.UserRoles.AddRange(userRoles);
        await _context.SaveChangesAsync();
        return true;
    }
}