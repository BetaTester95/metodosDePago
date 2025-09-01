using BilleterasBack.Wallets.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BilleterasBack.Wallets.Controllers
{
    [ApiController]
    [Route("/[Controller]")]
    public class MpController : ControllerBase
    {
        private readonly AppDbContext _context;
        public MpController(AppDbContext context)
        {
            _context = context;
        }

        [Authorize]
        [HttpPost("crearCuentaAppMp")]
        public async Task<IActionResult> CrearCuenta()
        {
            // Obtener id_usuario y dni desde el JWT
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "id_usuario")?.Value;
            var dniClaim = User.Claims.FirstOrDefault(c => c.Type == "dni")?.Value;

            if (userIdClaim == null || dniClaim == null)
                return Unauthorized("No se pudo obtener información del usuario");

            int userId = int.Parse(userIdClaim);
            int dni = int.Parse(dniClaim);

            // Obtener nombre y apellido desde la DB
            var usuario = await _context.Usuarios.FindAsync(userId);
            if (usuario == null)
                return NotFound("Usuario no encontrado");

            // Crear la cuenta de Mercado Pago
            var appMp = new AppMp(usuario.nombre, usuario.apellido, dni);

            var nuevaCuenta = new Mp
            {
                id_usuario = userId,
                nombre = usuario.nombre,
                apellido = usuario.apellido,
                dni = dni,
                cvu_mp = appMp.GenerarNumeroCbu(),
                saldo_cuenta_mercado_pago = 0.0m
            };

            _context.Mp.Add(nuevaCuenta);
            await _context.SaveChangesAsync();

            return Ok(nuevaCuenta);
        }


        [Authorize]
        [HttpPost("AgregarSaldoMp")]
        public async Task<IActionResult> AgregarDinero(int dni, [FromQuery] decimal monto)
        {
            var cuenta = await _context.Mp.FirstOrDefaultAsync(c => c.dni == dni);
            if(cuenta == null)
            {
                return NotFound("Cuenta no encontrada");
            }
            if(monto <= 0)
            {
                return BadRequest("El monto debe ser mayor a 0.");
            }
            cuenta.saldo_cuenta_mercado_pago += monto;
            await _context.SaveChangesAsync();
            return Ok(cuenta);
        }

        [HttpGet("saldo{dni}")]
        public async Task<IActionResult> ObtenerSaldo(int dni)
        {
            var cuenta = await _context.Mp.FirstOrDefaultAsync(_ => _.dni == dni);
            if(cuenta == null)
            {
                return NotFound("Cuenta no encontrada");
            }
            return Ok(cuenta.saldo_cuenta_mercado_pago);
        }


    }
}
