using System.Security.Cryptography;

namespace InmobiliariaMrAPI.Services;

public class FileService : IFileService
{
    private readonly IWebHostEnvironment _environment;

    public FileService(IWebHostEnvironment environment)
    {
        _environment = environment;
    }

    public async Task<string> SaveFileAsync(IFormFile file, string folder)
    {
        if (file == null || file.Length == 0)
        {
            throw new Exception("Archivo inválido");
        }

        // Validar tipo de archivo (solo imágenes)
        var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif", ".webp", ".heic", ".heif" };
        var fileExtension = Path.GetExtension(file.FileName).ToLowerInvariant();

        if (!allowedExtensions.Contains(fileExtension))
        {
            throw new Exception("Solo se permiten imágenes (jpg, jpeg, png, gif, webp, heic, heif)");
        }

        // Validar tamaño (máximo 5MB)
        if (file.Length > 5 * 1024 * 1024)
        {
            throw new Exception("La imagen no puede ser mayor a 5MB");
        }

        // Validar que WebRootPath exista, si no, crear wwwroot
        var webRootPath = _environment.WebRootPath;
        if (string.IsNullOrEmpty(webRootPath))
        {
            webRootPath = Path.Combine(_environment.ContentRootPath, "wwwroot");
            if (!Directory.Exists(webRootPath))
            {
                Directory.CreateDirectory(webRootPath);
            }
        }

        // Crear directorio si no existe
        var uploadsFolder = Path.Combine(webRootPath, folder);
        if (!Directory.Exists(uploadsFolder))
        {
            Directory.CreateDirectory(uploadsFolder);
        }

        // Generar nombre único para el archivo
        var fileName = $"{GenerateUniqueFileName()}{fileExtension}";
        var filePath = Path.Combine(uploadsFolder, fileName);

        // Guardar archivo
        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await file.CopyToAsync(stream);
        }

        // Devolver la ruta relativa para guardar en BD
        return $"/{folder}/{fileName}";
    }

    public bool DeleteFile(string filePath)
    {
        if (string.IsNullOrEmpty(filePath))
        {
            return false;
        }

        var webRootPath = _environment.WebRootPath ?? 
                         Path.Combine(_environment.ContentRootPath, "wwwroot");
        
        var fullPath = Path.Combine(webRootPath, filePath.TrimStart('/'));
        
        if (File.Exists(fullPath))
        {
            File.Delete(fullPath);
            return true;
        }

        return false;
    }

    private static string GenerateUniqueFileName()
    {
        return Guid.NewGuid().ToString("N").Substring(0, 16);
    }
}

