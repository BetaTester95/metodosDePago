using BilleterasBack.Wallets.Data;
using BilleterasBack.Wallets.Dtos;
using BilleterasBack.Wallets.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace BilleterasBack.Wallets.Servicios
{
    public class UsuarioService
    {
        private readonly AppDbContext _context;
        public UsuarioService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<object> crearUsuario(Usuario crearUsuario) //crear usuario
        {
            var emailExiste = await _context.Usuarios.AnyAsync(u => u.Email == crearUsuario.Email);
            if (emailExiste)
                return new { mensaje = "El email ya existe" };

            var dniExiste = await _context.Usuarios.AnyAsync(u => u.Dni == crearUsuario.Dni);
            if (dniExiste)
                return new { mensaje = "El DNI ya existe" };

            _context.Usuarios.Add(crearUsuario);
            await _context.SaveChangesAsync();
            return new { mensaje = "Usuario creado" };
        }

        public async Task<object> mostrarUsuarios() //mostrar usuarios
        {
            var listarUsuarios = await _context.Usuarios.Include(u => u.TipoUsuario).Where(u => u.Estado == "activo").Select(u => new
            {
                u.IdUsuario,
                u.Nombre,
                u.Apellido,
                u.Email,
                u.Dni,
                u.FechaRegistro,
                u.Estado,
                u.TipoUsuario.NombreTipo
            }).ToListAsync();

            if (listarUsuarios == null)
                return new { mensaje = "No hay usuarios para mostrar" };
            return listarUsuarios;
        }

        public async Task<object> borrarUsuario(int id) //borrar usuario
        {
            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario == null)
                return new { mensaje = "Usuario no encontrado" };
            _context.Usuarios.Remove(usuario);
            await _context.SaveChangesAsync();
            return new { mensaje = "Usuario eliminado" };
        }

        public async Task<object> editarUsuario(int id, UserDto usuarioActualizado) //editar usuario
        {
            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario == null)
                return new { mensaje = "Usuario no encontrado" };

            if (usuario.Email != usuarioActualizado.Email)
            {
                var emailExiste = await _context.Usuarios.AnyAsync(u => u.Email == usuarioActualizado.Email && u.IdUsuario != id);
                if (emailExiste)
                    return new { error = "email", mensaje = "El email ya existe" };
            }

            if (usuario.Dni != usuarioActualizado.Dni)
            {
                var dniExiste = await _context.Usuarios.AnyAsync(u => u.Dni == usuarioActualizado.Dni && u.IdUsuario != id);
                if (dniExiste)
                    return new { error = "dni", mensaje = "El DNI ya existe" };
            }

            usuario.Nombre = usuarioActualizado.Nombre;
            usuario.Apellido = usuarioActualizado.Apellido;
            usuario.Email = usuarioActualizado.Email;
            usuario.Dni = usuarioActualizado.Dni;
            await _context.SaveChangesAsync();
            return new { mensaje = "Usuario actualizado" };
        }
    }
}
