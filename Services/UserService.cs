using InmobiliariaMrAPI.Common;
using InmobiliariaMrAPI.DTOs;
using InmobiliariaMrAPI.Repositories;
using InmobiliariaMrAPI.Repositories.User;

namespace InmobiliariaMrAPI.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IRoleRepository _roleRepository;
    private readonly IPropietarioRepository _propietarioRepository;

    public UserService(IUserRepository userRepository, IRoleRepository roleRepository, IPropietarioRepository propietarioRepository)
    {
        _userRepository = userRepository;
        _roleRepository = roleRepository;
        _propietarioRepository = propietarioRepository;
    }

    public async Task<IEnumerable<Models.User.User>> GetAllUsers()
    {
        return await _userRepository.GetAllUsers();
    }

    public async Task<Result<UserDto>> GetUserById(int id)
    {
        var user = await _userRepository.GetUserById(id);
        if (user == null)
        {
            return Result<UserDto>.Fail("Usuario no encontrado");
        }
        
        var roles = await _roleRepository.GetRolesByUserId(user.Id);
        
        if (roles.Any(r => r.Name == "Propietario"))
        {
            var propietario = await _propietarioRepository.GetPropietarioByUserId(user.Id);
            var userDto = new UserDto()
            {
                Name = propietario.Name,
                LastName = propietario.LastName,
                Email = propietario.Email,
                ProfilePicRoute = user.ProfilePicRoute,
                Roles = roles.Select(r => r.Name).ToList(),
                Phone = propietario.Phone,
                DocumentNumber = propietario.DocumentNumber,
            };
            return Result<UserDto>.Ok(userDto);
        }
        
        var dto = new UserDto()
        {
            Name = user.Name,
            LastName = user.LastName,
            Email = user.Email,
            ProfilePicRoute = user.ProfilePicRoute,
            Roles = roles.Select(r => r.Name).ToList(),
        };
        return Result<UserDto>.Ok(dto);
    }

    public async Task<Models.User.User?> GetUserByEmail(string email)
    {
        return await _userRepository.GetUserByEmail(email);
    }

    public async Task<Models.User.User> CreateUser(Models.User.User user)
    {
        return await _userRepository.CreateUser(user);
    }

    public async Task<Result<UserDto>> UpdateLoggedUser(int userId, UserDto userDto)
    {
        var user = await _userRepository.GetUserById(userId);
        if (user == null)
        {
            return Result<UserDto>.Fail("Usuario no encontrado");
        }
        
        //? Actualizar user
        user.Name = userDto.Name ?? user.Name;
        user.LastName = userDto.LastName ?? user.LastName;
        user.ProfilePicRoute = userDto.ProfilePicRoute ?? "https://ui-avatars.com/api/?name=" + user.Name + "+" + user.LastName;
        user.UpdatedAt = DateTime.UtcNow;

        //? Actualizar propietario si tiene ese rol
        if (userDto.Roles.Contains("Propietario"))
        {
            var propietario = await _propietarioRepository.GetPropietarioByUserId(userId);
            if (propietario == null)
            {
                return Result<UserDto>.Fail("Propietario no encontrado");
            }
            propietario.Name = userDto.Name ?? propietario.Name;
            propietario.LastName = userDto.LastName ?? propietario.LastName;
            propietario.Phone = userDto.Phone ?? propietario.Phone;
            propietario.DocumentNumber = userDto.DocumentNumber ?? propietario.DocumentNumber;
            await _propietarioRepository.UpdatePropietario(propietario);
        }
        
        var updated = await _userRepository.UpdateUser(user);
        if (!updated)
        {
            return Result<UserDto>.Fail("Error al actualizar usuario");
        }
        
        var userUpdatedResult = await GetUserById(userId);
        return userUpdatedResult;
    }

    public async Task<Result<bool>> DeleteUser(int id)
    {
        var user = await _userRepository.GetUserById(id);
        if (user == null)
        {
            return Result<bool>.Fail("Usuario no encontrado");
        }
        
        var deleted = await _userRepository.DeleteUser(id);
        if (!deleted)
        {
            return Result<bool>.Fail("Error al eliminar usuario");
        }
        
        return Result<bool>.Ok(true);
    }
}