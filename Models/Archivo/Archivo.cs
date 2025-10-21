namespace InmobiliariaMrAPI.Models;

public class Archivo
{
    public int Id { get; set; }
    public string Nombre { get; set; } = null!;
    public string Ruta { get; set; } = null!;
    public DateTime Fecha { get; set; } = DateTime.UtcNow;

    //? Navigation Properties
    public ICollection<ArchivoInmueble> ArchivoInmuebles { get; set; } = new List<ArchivoInmueble>();
}