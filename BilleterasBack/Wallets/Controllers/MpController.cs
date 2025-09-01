using BilleterasBack.Wallets.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BilleterasBack.Wallets.Controllers
{
    [ApiController]
    [Route("api/[Controller]")]
    public class MpController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly AppMp _appMp;
        public MpController(AppDbContext context)
        {
            _context = context;
        }

        [Authorize]
        [HttpPost("crearCuentaAppMp")]
        public async Task<IActionResult> CrearCuenta()
        {
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "id_usuario")?.Value;
            if (userIdClaim == null) return Unauthorized();

            var usuario = await _context.Usuarios.FindAsync(int.Parse(userIdClaim));
            if (usuario == null) return NotFound();

            var cuenta = await _appMp.CrearCuenta(usuario);
            return Ok(cuenta);
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
