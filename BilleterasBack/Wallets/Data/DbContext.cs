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
        public DbSet<MovimientoBilletera> MovimientosBilletera { get; set; }
        public DbSet<TipoMovimiento> TiposMovimiento { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //  Índices unicos
            modelBuilder.Entity<Usuario>()
                .HasIndex(u => u.Email)
                .IsUnique();

            modelBuilder.Entity<Usuario>()
                .HasIndex(u => u.Dni)
                .IsUnique();

            //  Relaciones principales
            modelBuilder.Entity<Usuario>()
                .HasMany(u => u.Billeteras)
                .WithOne(b => b.Usuario)
                .HasForeignKey(b => b.IdUsuario)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Billetera>()
                .HasMany(b => b.Tarjetas)
                .WithOne(t => t.Billetera)
                .HasForeignKey(t => t.IdBilletera)
                .OnDelete(DeleteBehavior.Cascade);

            //  Relaciones nuevas (Movimientos)
            modelBuilder.Entity<TipoMovimiento>()
                .HasMany(tm => tm.Movimientos)
                .WithOne(m => m.TipoMovimiento)
                .HasForeignKey(m => m.IdTipoMovimiento)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Billetera>()
                .HasMany(b => b.Movimientos)
                .WithOne(m => m.Billetera)
                .HasForeignKey(m => m.IdBilletera)
                .OnDelete(DeleteBehavior.Cascade);

        }
    }
}