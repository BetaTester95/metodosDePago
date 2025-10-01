using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BilleterasBack.Wallets.Models;
using BilleterasBack.Wallets.Data;

[ApiController]
[Route("api/[controller]")]

public class UsuariosController : ControllerBase
{
    private readonly AppDbContext _context;

    public UsuariosController(AppDbContext context)
    {
        _context = context;
    }

    [HttpPost("registrar")]
    public async Task<IActionResult> GuardarUsuarios(Usuario usuario)
    {    
        try
        {
            bool maiLExiste = await _context.Usuarios.AnyAsync(u => u.Email == usuario.Email);
            if (maiLExiste)
            {
                return BadRequest(new
                {
                    Error = "CORREO_DUPLICADO",
                    Message = "El correo ya está registrado en el sistema"
                 
                });
            }
            bool dniExiste = await _context.Usuarios.AnyAsync(u => u.Dni == usuario.Dni);

            if (dniExiste)
            {
                return BadRequest(new
                {
                    error = "DNI_DUPLICADO",
                    message = "El DNI ya está registrado en el sistema",
                    field = "dni"
                });
            }

            if (string.IsNullOrEmpty(usuario.PasswordHash))
            {
                return BadRequest("Error la contraseña es obligatoria. ");
            }

            string passwordHash = BCrypt.Net.BCrypt.HashPassword(usuario.PasswordHash);
            usuario.PasswordHash = passwordHash;
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
