using InmobiliariaMrAPI.Common;
using InmobiliariaMrAPI.DTOs;
using InmobiliariaMrAPI.Models.User;

namespace InmobiliariaMrAPI.Services;

public interface IAuthService
{
    Task<Result<string>> Login(string email, string password);
    Task<Result<string>> Register(UserRegisterDto user);
    string ValidatePassword(string password);
}