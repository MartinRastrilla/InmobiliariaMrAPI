using InmobiliariaMrAPI.DTOs;
using InmobiliariaMrAPI.Models.User;

namespace InmobiliariaMrAPI.Services;

public interface IAuthService
{
    Task<bool> IsEmailValid(string email);
    string ValidatePassword(string password);
    string GenerateJwtToken(User user, List<string> roles);
    Task<string> Login(string email, string password);
    Task<string> Register(UserRegisterDto user);
}