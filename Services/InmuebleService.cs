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

    public async Task<Result<IEnumerable<InmuebleDto>>> GetAllInmueblesByUserId(int userId)
    {
        var propietario = await GetPropietarioByUserId(userId);
        if (propietario == null)
        {
            return Result<IEnumerable<InmuebleDto>>.Fail("Propietario no encontrado");
        }
        var inmuebles = await _inmuebleRepository.GetInmueblesByPropietarioId(propietario.Id);
        if (inmuebles == null || !inmuebles.Any())
        {
            return Result<IEnumerable<InmuebleDto>>.Fail("No inmuebles found for propietario");
        }
        var inmueblesDto = inmuebles.Select(i => MapInmuebleToDto(i)).ToList();
        return Result<IEnumerable<InmuebleDto>>.Ok(inmueblesDto);
    }

    public async Task<Result<InmuebleDto>> GetInmuebleById(int id, int userId)
    {
        var propietario = await GetPropietarioByUserId(userId);
        if (propietario == null)
        {
            return Result<InmuebleDto>.Fail("Propietario no encontrado");
        }
        var inmueble = await _inmuebleRepository.GetInmuebleById(id);
        if (inmueble == null)
        {
            return Result<InmuebleDto>.Fail("Inmueble no encontrado");
        }
        if (propietario.Id != inmueble.PropietarioId)
        {
            return Result<InmuebleDto>.Fail("No tienes permisos para ver este inmueble");
        }
        return Result<InmuebleDto>.Ok(MapInmuebleToDto(inmueble));
    }

    public async Task<Result<InmuebleDto>> GetInmuebleByTitle(string title, int userId)
    {
        var propietario = await GetPropietarioByUserId(userId);
        if (propietario == null)
        {
            return Result<InmuebleDto>.Fail("Propietario no encontrado");
        }
        var inmueble = await _inmuebleRepository.GetInmuebleByTitle(title);
        if (inmueble == null)
        {
            return Result<InmuebleDto>.Fail("Inmueble no encontrado");
        }

        if (propietario.Id != inmueble.PropietarioId)
        {
            return Result<InmuebleDto>.Fail("No tienes permisos para ver este inmueble");
        }
        return Result<InmuebleDto>.Ok(MapInmuebleToDto(inmueble));
    }

    public async Task<InmuebleDto?> CreateInmueble(InmuebleDto inmuebleDto, int propietarioId)
    {
        var propietario = await GetPropietarioByUserId(propietarioId);
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
        var propietario = await GetPropietarioByUserId(propietarioId);
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

    public async Task<Result<IEnumerable<InmuebleDto>>> GetInmueblesByPropietarioId(int propietarioId)
    {
        var inmuebles = await _inmuebleRepository.GetInmueblesByPropietarioId(propietarioId);
        if (inmuebles == null)
        {
            return Result<IEnumerable<InmuebleDto>>.Fail("No inmuebles found for propietario");
        }
        var inmueblesDto = inmuebles.Select(i => MapInmuebleToDto(i)).ToList();
        if (inmueblesDto.Count == 0)
        {
            return Result<IEnumerable<InmuebleDto>>.Fail("No inmuebles found for propietario");
        }
        return Result<IEnumerable<InmuebleDto>>.Ok(inmueblesDto);
    }

    public async Task<Result<bool>> DisableAndEnableInmuebleForUser(int id, int userId)
    {
        var propietario = await GetPropietarioByUserId(userId);
        if (propietario == null)
        {
            return Result<bool>.Fail("Propietario no encontrado");
        }
        var inmueble = await _inmuebleRepository.GetInmuebleById(id);
        if (inmueble == null)
        {
            return Result<bool>.Fail("Inmueble no encontrado");
        }

        if (inmueble.PropietarioId != propietario.Id)
        {
            return Result<bool>.Fail("No tienes permisos para habilitar este inmueble");
        }
        inmueble.Available = !inmueble.Available;
        var updated = await _inmuebleRepository.UpdateInmueble(inmueble);
        if (!updated)
        {
            return Result<bool>.Fail("Error al actualizar el inmueble");
        }
        return Result<bool>.Ok(updated);
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

    private async Task<Propietario?> GetPropietarioByUserId(int userId)
    {
        var propietario = await _propietarioRepository.GetPropietarioByUserId(userId);
        if (propietario == null)
        {
            return null;
        }
        return propietario;
    }
}