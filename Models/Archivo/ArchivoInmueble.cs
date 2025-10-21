namespace InmobiliariaMrAPI.Models;

public class ArchivoInmueble
{
    public int Id { get; set; }
    public int InmuebleId { get; set; }
    public Inmueble.Inmueble Inmueble { get; set; } = null!;
    public int ArchivoId { get; set; }
    public Archivo Archivo { get; set; } = null!;
}