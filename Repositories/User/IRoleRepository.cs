using InmobiliariaMrAPI.Models.User;

namespace InmobiliariaMrAPI.Repositories.User;

public interface IRoleRepository
{
    Task<IEnumerable<Role>> GetAllRoles();
    Task<Role> GetRoleById(int id);
    Task<Role> GetRoleByName(string name);
    Task<Role> CreateRole(Role role);
    Task<bool> UpdateRole(Role role);
    Task<bool> DeleteRole(int id);
    Task<bool> AddRolesToUser(int userId, List<string> roles);
    Task<IEnumerable<Role>> GetRolesByUserId(int userId);
}