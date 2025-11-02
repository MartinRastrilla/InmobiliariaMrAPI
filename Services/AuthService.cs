using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using InmobiliariaMrAPI.Common;
using InmobiliariaMrAPI.DTOs;
using InmobiliariaMrAPI.Models.Inmueble;
using InmobiliariaMrAPI.Models.User;
using InmobiliariaMrAPI.Repositories;
using InmobiliariaMrAPI.Repositories.User;
using Microsoft.IdentityModel.Tokens;

namespace InmobiliariaMrAPI.Services;

public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;
    private readonly IRoleRepository _roleRepository;
    private readonly IPropietarioRepository _propietarioRepository;
    private readonly IConfiguration _configuration;

    public AuthService(IUserRepository userRepository, IRoleRepository roleRepository, IPropietarioRepository propietarioRepository, IConfiguration configuration)
    {
        _userRepository = userRepository;
        _roleRepository = roleRepository;
        _propietarioRepository = propietarioRepository;
        _configuration = configuration;
    }

    public async Task<bool> IsEmailValid(string email)
    {
        return await _userRepository.GetUserByEmail(email) != null;
    }

    public bool IsPasswordValid(string password, string hashedPassword)
    {
        return BCrypt.Net.BCrypt.Verify(password, hashedPassword);
    }

    private string HashPassword(string password)
    {
        return BCrypt.Net.BCrypt.HashPassword(password);
    }

    public string GenerateJwtToken(User user, List<string> roles)
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Name, user.Name)
        };
        
        // Agregar roles como claims
        foreach (var role in roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
        }
        
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        
        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: claims,
            expires: DateTime.Now.AddMinutes(Convert.ToDouble(_configuration["Jwt:ExpiryInMinutes"])),
            signingCredentials: credentials
        );
        
        return new JwtSecurityTokenHandler().WriteToken(token);
    }


    public async Task<Result<string>> Login(string email, string password)
    {
        var user = await _userRepository.GetUserByEmail(email);
        if (user == null)
        {
            return Result<string>.Fail("Usuario no encontrado");
        }

        if (!IsPasswordValid(password, user.Password))
        {
            return Result<string>.Fail("Contraseña incorrecta");
        }

        var roles = await _roleRepository.GetRolesByUserId(user.Id);
        if (!roles.Any())
        {
            return Result<string>.Fail("El usuario no tiene roles asignados");
        }
        
        var token = GenerateJwtToken(user, roles.Select(r => r.Name).ToList());
        return Result<string>.Ok(token);
    }

    public async Task<Result<string>> Register(UserRegisterDto userDto)
    {
        //? Validar que los roles existan
        if (!await ValidateRoles(userDto.Roles))
        {
            return Result<string>.Fail("Roles inválidos");
        }
        
        //? Validar que el email no esté en uso
        if (await IsEmailValid(userDto.Email))
        {
            return Result<string>.Fail("El email ya está en uso");
        }
        
        //? Validar que la contraseña tenga al menos 8 caracteres
        var validation = ValidatePassword(userDto.Password);
        if (validation != "Password is valid")
        {
            return Result<string>.Fail(validation);
        }

        //? Crear el usuario
        var user = new User
        {
            Name = userDto.Name,
            LastName = userDto.LastName,
            Email = userDto.Email,
            Password = HashPassword(userDto.Password),
            ProfilePicRoute = $"https://ui-avatars.com/api/?name={userDto.Name}+{userDto.LastName}&background=8E02AA&color=fff&size=256"
        };
        user = await _userRepository.CreateUser(user);
        
        //? Agregar roles al usuario
        if (!await _roleRepository.AddRolesToUser(user.Id, userDto.Roles))
        {
            await _userRepository.DeleteUser(user.Id);
            return Result<string>.Fail("Error al asignar roles al usuario");
        }

        //? Validar que el usuario tenga el rol de Propietario
        if (await VerifyPropietarioRole(userDto.Roles))
        {
            var userPropietario = new Propietario
            {
                UserId = user.Id,
                Name = userDto.Name,
                LastName = userDto.LastName,
                Email = userDto.Email,
                Phone = userDto.Phone ?? string.Empty,
                DocumentNumber = userDto.DocumentNumber!
            };
            await _propietarioRepository.CreatePropietario(userPropietario);
        }
        
        return Result<string>.Ok(user.Id.ToString());
    }

    public string ValidatePassword(string password)
    {
        //? Validar que la contraseña tenga al menos 8 caracteres
        if (password.Length < 8)
        {
            return "Password must be at least 8 characters long";
        }

        //? Validar que la contraseña tenga al menos una letra mayúscula
        if (!password.Any(char.IsUpper))
        {
            return "Password must contain at least one uppercase letter";
        }

        //? Validar que la contraseña tenga al menos un número
        if (!password.Any(char.IsDigit))
        {
            return "Password must contain at least one number";
        }
        return "Password is valid";
    }

    private async Task<bool> ValidateRoles(List<string> roles)
    {
        foreach (var role in roles)
        {
            var roleEntity = await _roleRepository.GetRoleByName(role);
            if (roleEntity == null) return false;
        }
        return true;
    }

    private async Task<bool> VerifyPropietarioRole(List<string> roles)
    {
        foreach (var role in roles)
        {
            var roleEntity = await _roleRepository.GetRoleByName(role);
            if (roleEntity == null) return false;
            if (roleEntity.Name == "Propietario") return true;
        }
        return false;
    }
}