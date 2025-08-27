using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;
using BCrypt.Net;

[ApiController]
[Route("api/[controller]")]
public class UsuariosController : ControllerBase
{
    private readonly AppDbContext _context;
    public UsuariosController(AppDbContext context)
    {
        _context = context;
    }

    [HttpPost]
    public async Task<IActionResult> GuardarUsuarios(Usuario usuario)
    {
        try
        {
            bool maiLExiste = await _context.Usuarios.AnyAsync(u => u.email == usuario.email);
            if (maiLExiste)
            {
                return BadRequest(new
                {
                    error = "CORREO_DUPLICADO",
                    message = "El correo ya está registrado en el sistema",
                    field = "correo"
                });
            }

            // Verificar si el DNI ya existe
            bool dniExiste = await _context.Usuarios.AnyAsync(u => u.dni == usuario.dni);

            if (dniExiste)
            {
                return BadRequest(new
                {
                    error = "DNI_DUPLICADO",
                    message = "El DNI ya está registrado en el sistema",
                    field = "dni"
                });
            }

            if(string.IsNullOrEmpty(usuario.nombre) || string.IsNullOrEmpty(usuario.apellido))
            {
                return BadRequest(new
                {
                    error = "Error al validar el nombre o apellido",
                    message = "El nombre o apellido no puede estar vacio",
                    field = "nombre, apellido"
                });
            }

            if(string.IsNullOrEmpty(usuario.password_hash))
            {
                return BadRequest("Error la contraseña es obligatoria. ");
            }

            string passwordHash = BCrypt.Net.BCrypt.HashPassword(usuario.password_hash);
            usuario.password_hash = passwordHash;

            _context.Usuarios.Add(usuario);
            await _context.SaveChangesAsync();

            var response = new
            {
                StatusCode = 201,
                Message = "Usuario guardado correctamente",
                Data = usuario
            };
            return new JsonResult(response) { StatusCode = 201 };
        }
        catch (Exception ex)
        {
            var error = new
            {
                StatusCode = 500,
                Message = "Error al guardar el usuario",
                Error = ex.Message
            };
            return new JsonResult(error) { StatusCode = 500 };
        }
    }
}
