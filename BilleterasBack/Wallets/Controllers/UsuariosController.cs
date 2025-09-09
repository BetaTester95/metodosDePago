using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;
using BCrypt.Net;
using BilleterasBack.Wallets.Models;
using BilleterasBack.Wallets.PayPal;
using System.Text.RegularExpressions;
using BilleterasBack.Wallets.Validaciones;

[ApiController]
[Route("api/[controller]")]

public class UsuariosController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly Validador _validador = new Validador();
    public UsuariosController(AppDbContext context)
    {
        _context = context;
    }

    [HttpPost("registrar")]
    public async Task<IActionResult> GuardarUsuarios(Usuario usuario)
    {
        if(_validador.validarNombre(usuario.Nombre) || _validador.validarApellido(usuario.Apellido))
        {
            return BadRequest(new
            {
                error = "NOMBRE_APELLIDO_INVALIDO",
                message = "El nombre o apellido no puede estar vacio",
                field = "nombre, apellido"
            });
        }


        if (!_validador.validarDNI(usuario.Dni))
        {
            return BadRequest(new
            {
                error = "DNI_INVALIDO",
                message = "El DNIdebe tener hasta 8 digitos",
                field = "dni"
            });
        }

        try
        {
            bool maiLExiste = await _context.Usuarios.AnyAsync(u => u.Email == usuario.Email);
            if (maiLExiste)
            {
                return BadRequest(new
                {
                    error = "CORREO_DUPLICADO",
                    message = "El correo ya está registrado en el sistema",
                    field = "correo"
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
