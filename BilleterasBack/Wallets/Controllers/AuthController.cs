using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly string jwtKey = "estaEsUnaClaveSuperSecreta123333!!";
    public AuthController(AppDbContext context)
    {
        _context = context;
    }
  
    [HttpPost("login")]
    public async Task<IActionResult> Login(Usuario loginUsuario)
    {
        var usuario = await _context.Usuarios.FirstOrDefaultAsync(u=> u.email == loginUsuario.email);
        if(usuario == null)
        {
            return Unauthorized(new {message ="correo o contraseña incorrecta"});
        }
        bool passValida = BCrypt.Net.BCrypt.Verify(loginUsuario.password_hash, usuario.password_hash);

        if (!passValida)
        {
            return Unauthorized(new {message = "correo o contraseña incorrecta"});
        }
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(jwtKey);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                    new Claim("id_usuario", usuario.id_usuario.ToString()), // <-- ahora se llama id_usuario
                    new Claim("rol",usuario.tipo_usuario),
                    new Claim("dni", usuario.dni.ToString()),               // <-- agregamos el DNI
                    new Claim(ClaimTypes.Email, usuario.email)
                }),
            Expires = DateTime.UtcNow.AddHours(1),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);
        var tokenString = tokenHandler.WriteToken(token);

        return Ok(new {message="Login exitoso",
            token = tokenString
        });
    }


    [HttpGet("test")]
    public IActionResult Test()
    {
        return Ok(new { mensaje = "Funciona" });
    }

}