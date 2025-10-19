using InmobiliariaMrAPI.Models.User;

namespace InmobiliariaMrAPI.Repositories.User;

public interface IUserRepository
{
    Task<IEnumerable<Models.User.User>> GetAllUsers();
    Task<Models.User.User> GetUserById(int id);
    Task<Models.User.User> GetUserByEmail(string email);
    Task<Models.User.User> CreateUser(Models.User.User user);
    Task<bool> UpdateUser(Models.User.User user);
    Task<bool> DeleteUser(int id);
}