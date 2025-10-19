namespace InmobiliariaMrAPI.DTOs;

public class UserDto
{
    public string Name { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Password { get; set; } = null!;
    public List<string> Roles { get; set; } = new List<string>();
}

public class UserRegisterDto
{
    public string Name { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Password { get; set; } = null!;
    public List<string> Roles { get; set; } = new List<string>();

    //? Propietario
    public string? Phone { get; set; }
    public string? DocumentNumber { get; set; }
}

public class UserLoginDto
{
    public string Email { get; set; } = null!;
    public string Password { get; set; } = null!;
}