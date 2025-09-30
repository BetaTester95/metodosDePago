using BilleterasBack.Wallets.Data;
using BilleterasBack.Wallets.Dtos;
using BilleterasBack.Wallets.Models;
using BilleterasBack.Wallets.Validaciones;
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

        public async Task<Resultado<Usuario>> crearUsuario(Usuario nuevoUsuario) //crear usuario
        {
            var emailExiste = await _context.Usuarios.AnyAsync(u => u.Email == nuevoUsuario.Email);
            if (emailExiste)
                return Resultado<Usuario>.Failure("El email ya esta registrado.");

            var dniExiste = await _context.Usuarios.AnyAsync(u => u.Dni == nuevoUsuario.Dni);
            if (dniExiste)
                return Resultado<Usuario>.Failure("El dni ya esta registrados");

            try
            {
                _context.Usuarios.Add(nuevoUsuario);
                await _context.SaveChangesAsync();
                return Resultado<Usuario>.Success(nuevoUsuario);
            }
            catch(Exception ex)
            {
                return Resultado<Usuario>.Failure("Error al crear un usuario: " + ex.Message);
            }
        }

        public async Task<List<Usuario>> mostrarUsuarios() //mostrar usuarios
        {
            var listarUsuarios = await _context.Usuarios
            .Include(u => u.TipoUsuario)
            .Where(u => u.Estado == "activo")
            .ToListAsync();

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

        public async Task<UserDto> editarUsuario(int id, UserDto usuarioActualizado)
        {      
            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario == null)
                return new UserDto { Message = "No se encontro ningun usuario" };

            if (usuario.Email != usuarioActualizado.Email)
            {
                var emailExiste = await _context.Usuarios.AnyAsync(u => u.Email == usuarioActualizado.Email && u.IdUsuario != id);
                if (emailExiste)
                    return new UserDto { Success = false, Message="El mail existe" };
            }

            if (usuario.Dni != usuarioActualizado.Dni)
            {
                var dniExiste = await _context.Usuarios.AnyAsync(u => u.Dni == usuarioActualizado.Dni && u.IdUsuario != id);
                if (dniExiste)
                    return new UserDto { Success = false, Message = $"El dni {usuario.Dni} esta siendo usado" };
            }
            var dataTemp= new UserDto
            {
                Nombre = usuarioActualizado.Nombre,
                Apellido = usuarioActualizado.Apellido,
                Email = usuarioActualizado.Email,
                Dni = usuarioActualizado.Dni
            };

            usuario.Nombre = usuarioActualizado.Nombre;
            usuario.Apellido = usuarioActualizado.Apellido;
            usuario.Email = usuarioActualizado.Email;
            usuario.Dni = usuarioActualizado.Dni;
            await _context.SaveChangesAsync();
            return dataTemp;
        }
    }
}
