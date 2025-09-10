using BilleterasBack.Wallets.Data;
using BilleterasBack.Wallets.Models;
using Microsoft.EntityFrameworkCore;


namespace BilleterasBack.Wallets.Servicios
{
    public class UsuarioService
    {
        private readonly AppDbContext _context;
        public UsuarioService(AppDbContext context) {
            _context = context;
        }


        public async Task<object> mostrarUsuarios() //mostrar usuarios
        {
            var listarUsuarios = await _context.Usuarios.Include(u => u.TipoUsuario).Where(u=> u.Estado == "activo").Select(u=> new
            {
                u.IdTipoUsuario,
                u.Nombre,
                u.Apellido,
                u.Email,
                u.Dni,
                u.FechaRegistro,
                u.Estado,
                TipoUsuario = new { 
                    u.TipoUsuario.IdTipoUsuario,
                    u.TipoUsuario.NombreTipo
                }
            }).ToListAsync();

            if (listarUsuarios == null)
                return new { mensaje = "No hay usuarios para mostrar" };

            return listarUsuarios;
        }
    }
}
