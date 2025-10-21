using InmobiliariaMrAPI.Common;
using InmobiliariaMrAPI.DTOs;
using InmobiliariaMrAPI.Models.Inmueble;
using InmobiliariaMrAPI.Repositories;
using InmobiliariaMrAPI.Repositories.User;

namespace InmobiliariaMrAPI.Services;

public class InmuebleService : IInmuebleService
{
    private readonly IInmuebleRepository _inmuebleRepository;
    private readonly IPropietarioRepository _propietarioRepository;
    private readonly IUserRepository _userRepository;

    public InmuebleService(IInmuebleRepository inmuebleRepository, IPropietarioRepository propietarioRepository, IUserRepository userRepository)
    {
        _inmuebleRepository = inmuebleRepository;
        _propietarioRepository = propietarioRepository;
        _userRepository = userRepository;
    }

    public async Task<IEnumerable<InmuebleDto>> GetAllInmuebles()
    {
        var inmuebles = await _inmuebleRepository.GetAllInmuebles();
        return inmuebles.Select(i => MapInmuebleToDto(i)).ToList() ?? throw new Exception("No inmuebles found");
    }

    public async Task<Result<InmuebleDto>> GetInmuebleById(int id)
    {
        var inmueble = await _inmuebleRepository.GetInmuebleById(id);
        if (inmueble == null)
        {
            return Result<InmuebleDto>.Fail("Inmueble no encontrado");
        }
        return Result<InmuebleDto>.Ok(MapInmuebleToDto(inmueble));
    }

    public async Task<Result<InmuebleDto>> GetInmuebleByTitle(string title)
    {
        var inmueble = await _inmuebleRepository.GetInmuebleByTitle(title);
        if (inmueble == null)
        {
            return Result<InmuebleDto>.Fail("Inmueble no encontrado");
        }
        return Result<InmuebleDto>.Ok(MapInmuebleToDto(inmueble));
    }

    public async Task<InmuebleDto?> CreateInmueble(InmuebleDto inmuebleDto, int propietarioId)
    {
        var propietario = await GetPropietarioById(propietarioId);
        if (propietario == null)
        {
            return null;
        }
        var inmueble = new Inmueble
        {
            Title = inmuebleDto.Title,
            Address = inmuebleDto.Address,
            Latitude = inmuebleDto.Latitude,
            Longitude = inmuebleDto.Longitude,
            Rooms = inmuebleDto.Rooms,
            Price = inmuebleDto.Price,
            MaxGuests = inmuebleDto.MaxGuests,
            Available = inmuebleDto.Available,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
            PropietarioId = propietario.Id,
        };
        inmueble = await _inmuebleRepository.CreateInmueble(inmueble);
        return MapInmuebleToDto(inmueble);
    }

    public async Task<bool> UpdateInmueble(InmuebleDto inmuebleDto, int propietarioId)
    {
        var propietario = await GetPropietarioById(propietarioId);
        if (propietario == null)
        {
            return false;
        }
        var inmueble = await _inmuebleRepository.GetInmuebleById(inmuebleDto.Id);
        if (inmueble == null)
        {
            return false;
        }
        if (inmueble.PropietarioId != propietario.Id)
        {
            return false;
        }
        inmueble.Title = inmuebleDto.Title ?? inmueble.Title;
        inmueble.Address = inmuebleDto.Address ?? inmueble.Address;
        inmueble.Latitude = inmuebleDto.Latitude ?? inmueble.Latitude;
        inmueble.Longitude = inmuebleDto.Longitude ?? inmueble.Longitude;
        inmueble.Rooms = inmuebleDto.Rooms > 0 ? inmuebleDto.Rooms : inmueble.Rooms;
        inmueble.Price = inmuebleDto.Price > 0 ? inmuebleDto.Price : inmueble.Price;
        inmueble.MaxGuests = inmuebleDto.MaxGuests > 0 ? inmuebleDto.MaxGuests : inmueble.MaxGuests;
        inmueble.Available = inmuebleDto.Available;
        inmueble.UpdatedAt = DateTime.UtcNow;
        return await _inmuebleRepository.UpdateInmueble(inmueble);
    }

    public async Task<Result<bool>> DeleteInmueble(int id)
    {
        var inmueble = await _inmuebleRepository.GetInmuebleById(id);
        if (inmueble == null)
        {
            return Result<bool>.Fail("Inmueble no encontrado");
        }
        
        var deleted = await _inmuebleRepository.DeleteInmueble(id);
        return Result<bool>.Ok(deleted);
    }

    // Métodos con Result Pattern para controllers
    public async Task<Result<InmuebleDto>> CreateInmuebleForUser(InmuebleDto inmuebleDto, int userId)
    {
        // Obtener propietario desde userId
        var propietario = await _propietarioRepository.GetPropietarioByUserId(userId);
        if (propietario == null)
        {
            return Result<InmuebleDto>.Fail("No estás registrado como propietario");
        }
        
        // Validaciones
        if (string.IsNullOrWhiteSpace(inmuebleDto.Title))
        {
            return Result<InmuebleDto>.Fail("El título es requerido");
        }
        
        if (inmuebleDto.Price <= 0)
        {
            return Result<InmuebleDto>.Fail("El precio debe ser mayor a 0");
        }
        
        // Crear inmueble
        var inmueble = new Inmueble
        {
            Title = inmuebleDto.Title,
            Address = inmuebleDto.Address,
            Latitude = inmuebleDto.Latitude,
            Longitude = inmuebleDto.Longitude,
            Rooms = inmuebleDto.Rooms,
            Price = inmuebleDto.Price,
            MaxGuests = inmuebleDto.MaxGuests,
            Available = inmuebleDto.Available,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
            PropietarioId = propietario.Id,
        };
        
        inmueble = await _inmuebleRepository.CreateInmueble(inmueble);
        return Result<InmuebleDto>.Ok(MapInmuebleToDto(inmueble));
    }

    public async Task<Result<InmuebleDto>> UpdateInmuebleForUser(InmuebleDto inmuebleDto, int userId)
    {
        // Obtener propietario desde userId
        var propietario = await _propietarioRepository.GetPropietarioByUserId(userId);
        if (propietario == null)
        {
            return Result<InmuebleDto>.Fail("No estás registrado como propietario");
        }
        
        // Verificar que el inmueble existe
        var inmueble = await _inmuebleRepository.GetInmuebleById(inmuebleDto.Id);
        if (inmueble == null)
        {
            return Result<InmuebleDto>.Fail("Inmueble no encontrado");
        }
        
        // Verificar permisos
        if (inmueble.PropietarioId != propietario.Id)
        {
            return Result<InmuebleDto>.Fail("No tienes permiso para actualizar este inmueble");
        }
        
        // Actualizar
        inmueble.Title = inmuebleDto.Title ?? inmueble.Title;
        inmueble.Address = inmuebleDto.Address ?? inmueble.Address;
        inmueble.Latitude = inmuebleDto.Latitude ?? inmueble.Latitude;
        inmueble.Longitude = inmuebleDto.Longitude ?? inmueble.Longitude;
        inmueble.Rooms = inmuebleDto.Rooms > 0 ? inmuebleDto.Rooms : inmueble.Rooms;
        inmueble.Price = inmuebleDto.Price > 0 ? inmuebleDto.Price : inmueble.Price;
        inmueble.MaxGuests = inmuebleDto.MaxGuests > 0 ? inmuebleDto.MaxGuests : inmueble.MaxGuests;
        inmueble.Available = inmuebleDto.Available;
        inmueble.UpdatedAt = DateTime.UtcNow;
        
        await _inmuebleRepository.UpdateInmueble(inmueble);
        return Result<InmuebleDto>.Ok(MapInmuebleToDto(inmueble));
    }

    public async Task<IEnumerable<InmuebleDto>> GetInmueblesByPropietarioId(int propietarioId)
    {
        var inmuebles = await _inmuebleRepository.GetInmueblesByPropietarioId(propietarioId);
        return inmuebles.Select(i => MapInmuebleToDto(i)).ToList() ?? throw new Exception("No inmuebles found for propietario");
    }

    public async Task<InmuebleDto?> GetInmuebleByTitleForPropietario(string title, int propietarioId)
    {
        var inmueble = await _inmuebleRepository.GetInmuebleByTitleForPropietario(title, propietarioId);
        return MapInmuebleToDto(inmueble);
    }

    public async Task<bool> IsInmuebleAvailable(int id)
    {
        var inmueble = await _inmuebleRepository.GetInmuebleById(id);
        return inmueble.Available;
    }

    private static InmuebleDto MapInmuebleToDto(Inmueble inmueble)
    {
        return new InmuebleDto
        {
            Id = inmueble.Id,
            Title = inmueble.Title,
            Address = inmueble.Address,
            Latitude = inmueble.Latitude,
            Longitude = inmueble.Longitude,
            Rooms = inmueble.Rooms,
            Price = inmueble.Price,
            MaxGuests = inmueble.MaxGuests,
            Available = inmueble.Available,
            CreatedAt = inmueble.CreatedAt,
            UpdatedAt = inmueble.UpdatedAt,
        };
    }

    private async Task<Propietario?> GetPropietarioById(int propietarioId)
    {
        var propietario = await _propietarioRepository.GetPropietarioById(propietarioId);
        if (propietario == null)
        {
            return null;
        }
        return propietario;
    }
}