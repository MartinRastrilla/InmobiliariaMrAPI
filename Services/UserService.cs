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
    private readonly IFileService _fileService;
    public UserService(IUserRepository userRepository, IRoleRepository roleRepository, IPropietarioRepository propietarioRepository, IFileService fileService)
    {
        _userRepository = userRepository;
        _roleRepository = roleRepository;
        _propietarioRepository = propietarioRepository;
        _fileService = fileService;
    }

    public async Task<Result<IEnumerable<UserDto>>> GetAllUsers()
    {
        var users = await _userRepository.GetAllUsers();
        
        if (users == null || !users.Any())
        {
            return Result<IEnumerable<UserDto>>.Fail("No se encontraron usuarios");
        }

        var usersDto = new List<UserDto>();
        foreach (var user in users)
        {
            var roles = await _roleRepository.GetRolesByUserId(user.Id);
            var userDto = new UserDto
            {
                Name = user.Name,
                LastName = user.LastName,
                Email = user.Email,
                ProfilePicRoute = user.ProfilePicRoute,
                Roles = roles.Select(r => r.Name).ToList()
            };
            
            // Si tiene rol de Propietario, agregar información adicional
            if (roles.Any(r => r.Name == "Propietario"))
            {
                var propietario = await _propietarioRepository.GetPropietarioByUserId(user.Id);
                if (propietario != null)
                {
                    userDto.Phone = propietario.Phone;
                    userDto.DocumentNumber = propietario.DocumentNumber;
                }
            }
            
            usersDto.Add(userDto);
        }
        
        return Result<IEnumerable<UserDto>>.Ok(usersDto);
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
                Name = propietario!.Name,
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

    public async Task<Result<UserDto>> UpdateLoggedUser(int userId, UserDto userDto, IFormFile? profilePic = null)
    {
        var user = await _userRepository.GetUserById(userId);
        if (user == null)
        {
            return Result<UserDto>.Fail("Usuario no encontrado");
        }

        // Si hay nueva imagen, guardarla
        if (profilePic != null)
        {
            // Eliminar imagen anterior si existe
            if (!string.IsNullOrEmpty(user.ProfilePicRoute) && !user.ProfilePicRoute.StartsWith("http"))
            {
                _fileService.DeleteFile(user.ProfilePicRoute);
            }

            try
            {
                user.ProfilePicRoute = await _fileService.SaveFileAsync(profilePic, "uploads/profiles");
            }
            catch (Exception ex)
            {
                return Result<UserDto>.Fail(ex.Message);
            }
        }
        
        //? Actualizar user
        user.Name = userDto.Name ?? user.Name;
        user.LastName = userDto.LastName ?? user.LastName;
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

    public async Task<Result<UserDto>> UpdatePassword(int userId, UserUpdatePasswordDto userUpdatePasswordDto)
    {
        var user = await _userRepository.GetUserById(userId);
        if (user == null)
        {
            return Result<UserDto>.Fail("Usuario no encontrado");
        }

        //? Validar que las contraseñas sean iguales
        if (userUpdatePasswordDto.NewPassword != userUpdatePasswordDto.ConfirmPassword)
        {
            return Result<UserDto>.Fail("Las contraseñas no coinciden");
        }

        //? Validar que la current password sea correcta
        if (!BCrypt.Net.BCrypt.Verify(userUpdatePasswordDto.CurrentPassword, user.Password))
        {
            return Result<UserDto>.Fail("Contraseña actual incorrecta");
        }

        // Validar que la nueva contraseña sea diferente a la actual
        if (BCrypt.Net.BCrypt.Verify(userUpdatePasswordDto.NewPassword, user.Password))
        {
            return Result<UserDto>.Fail("La nueva contraseña debe ser diferente a la actual");
        }

        // Actualizar contraseña
        user.Password = BCrypt.Net.BCrypt.HashPassword(userUpdatePasswordDto.NewPassword);
        var updated = await _userRepository.UpdateUser(user);
        
        if (!updated)
        {
            return Result<UserDto>.Fail("Error al actualizar la contraseña");
        }

        var userUpdatedResult = await GetUserById(userId);
        return userUpdatedResult;
    }
}