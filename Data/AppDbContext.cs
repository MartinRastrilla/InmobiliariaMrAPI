using InmobiliariaMrAPI.Models.Inmueble;
using InmobiliariaMrAPI.Models.User;
using Microsoft.EntityFrameworkCore;

namespace InmobiliariaMrAPI.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<User> Users { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<UserRole> UserRoles { get; set; }
    public DbSet<Propietario> Propietarios { get; set; }
    public DbSet<Inmueble> Inmuebles { get; set; }
    public DbSet<Contrato> Contratos { get; set; }
    public DbSet<Pagos> Pagos { get; set; }
    public DbSet<Inquilino> Inquilinos { get; set; }
    public DbSet<ContratoInquilino> ContratoInquilinos { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.Entity<UserRole>()
            .HasIndex(ur => new { ur.UserId, ur.RoleId })
            .IsUnique();

        modelBuilder.Entity<User>()
            .HasIndex(u => u.Email)
            .IsUnique();

        modelBuilder.Entity<Propietario>()
            .HasIndex(p => p.DocumentNumber)
            .IsUnique();

        modelBuilder.Entity<Inquilino>()
            .HasIndex(i => i.DocumentNumber)
            .IsUnique();

        modelBuilder.Entity<ContratoInquilino>()
            .HasIndex(ci => new { ci.ContratoId, ci.InquilinoId })
            .IsUnique();
    }
}