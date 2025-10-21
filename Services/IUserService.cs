using InmobiliariaMrAPI.Common;
using InmobiliariaMrAPI.DTOs;

namespace InmobiliariaMrAPI.Services;

public interface IUserService
{
    Task<IEnumerable<Models.User.User>> GetAllUsers();
    Task<Result<UserDto>> GetUserById(int id);
    Task<Result<UserDto>> UpdateLoggedUser(int userId, UserDto userDto);
    Task<Result<bool>> DeleteUser(int id);
}