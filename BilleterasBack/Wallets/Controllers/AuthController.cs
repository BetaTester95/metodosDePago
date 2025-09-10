using BilleterasBack.Wallets.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BilleterasBack.Wallets.Dtos;
using BilleterasBack.Wallets.Data;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly AppDbContext _context;
    public AuthController(AppDbContext context)
    {
        _context = context;
    }
  
    //[HttpPost("login")]
    //public async Task<IActionResult> Login(LoginDTO loginUsuario)
    //{
    //    var usuario = await _context.Usuarios
    //                        .Include(u => u.TipoUsuario)
    //                        .FirstOrDefaultAsync(u => u.Email == loginUsuario.Email);
    //    if (usuario == null)
    //    {
    //        return Unauthorized(new {message ="correo o contraseña incorrecta"});
    //    }
    //    bool passValida = BCrypt.Net.BCrypt.Verify(loginUsuario.PasswordHash, usuario.PasswordHash);

    //    if (!passValida)
    //    {
    //        return Unauthorized(new {message = "correo o contraseña incorrecta"});
    //    }      
    //    return Ok(new {message="Login exitoso",
            
    //    });
    //}
}