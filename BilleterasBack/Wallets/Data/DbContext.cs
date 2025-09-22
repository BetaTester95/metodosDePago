using BilleterasBack.Wallets.Models;
using Microsoft.EntityFrameworkCore;

namespace BilleterasBack.Wallets.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<TipoUsuario> TiposUsuario { get; set; }
        public DbSet<Billetera> Billeteras { get; set; }
        public DbSet<Tarjeta> Tarjetas { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Índices únicos
            modelBuilder.Entity<Usuario>()
                .HasIndex(u => u.Email)
                .IsUnique();

            modelBuilder.Entity<Usuario>()
                .HasIndex(u => u.Dni)
                .IsUnique();

            // Relaciones
            modelBuilder.Entity<Usuario>()
                .HasMany(u => u.Billeteras)
                .WithOne(b => b.Usuario)
                .HasForeignKey(b => b.IdUsuario);

            modelBuilder.Entity<Billetera>()
                .HasMany(b => b.Tarjetas)
                .WithOne(t => t.Billetera)
                .HasForeignKey(t => t.IdBilletera);

        }
    }
}