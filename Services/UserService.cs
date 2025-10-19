using InmobiliariaMrAPI.Repositories.User;

namespace InmobiliariaMrAPI.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;

    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<IEnumerable<Models.User.User>> GetAllUsers()
    {
        return await _userRepository.GetAllUsers();
    }

    public async Task<Models.User.User> GetUserById(int id)
    {
        return await _userRepository.GetUserById(id);
    }

    public async Task<Models.User.User> GetUserByEmail(string email)
    {
        return await _userRepository.GetUserByEmail(email);
    }

    public async Task<Models.User.User> CreateUser(Models.User.User user)
    {
        return await _userRepository.CreateUser(user);
    }

    public async Task<bool> UpdateUser(Models.User.User user)
    {
        return await _userRepository.UpdateUser(user);
    }

    public async Task<bool> DeleteUser(int id)
    {
        return await _userRepository.DeleteUser(id);
    }
}