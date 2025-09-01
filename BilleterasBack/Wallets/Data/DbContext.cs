using BilleterasBack.Wallets.Models;
using Microsoft.EntityFrameworkCore;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Usuario> Usuarios { get; set; }
    public DbSet<Mp> Mp { get; set; }
    public DbSet<TarjetaEntity> Tarjetas { get; set; } // <- agregar el DbSet de Tarjeta

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Configurar Dni como �nico en Usuario
        modelBuilder.Entity<Usuario>()
            .HasIndex(u => u.dni)
            .IsUnique();

        // Opcional: Configurar un �ndice �nico por num_tarjeta si quieres evitar duplicados
        modelBuilder.Entity<TarjetaEntity>()
            .HasIndex(t => t.numeroTarjeta)
            .IsUnique();

        // Configuraci�n de la relaci�n Usuario-Tarjeta
        modelBuilder.Entity<TarjetaEntity>()
            .HasOne(t => t.Usuario)
            .WithMany()
            .HasForeignKey(t => t.id_usuario)
            .OnDelete(DeleteBehavior.SetNull);
    }
}
