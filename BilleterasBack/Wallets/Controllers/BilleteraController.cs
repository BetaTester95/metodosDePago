using EjercicioInterfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BilleterasBack.Wallets.Controllers
{

    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class BilleteraController : ControllerBase
    {

        private readonly AppDbContext _context;
        private readonly AppMp _appMp;
        private readonly CuentaDni _cuentaDni;
        private readonly PayPal _payPal;
        public BilleteraController(AppDbContext context)
        {
            _context = context;
            _appMp = new AppMp(_context);
            _cuentaDni = new CuentaDni(_context);
            _payPal = new PayPal(_context);
        }


        [HttpPost("crear/mercadopago")]
        public async Task<IActionResult> CrearMercadoPago()
        {
            var userId = int.Parse(User.Claims.First(c => c.Type == "id_usuario").Value);
            var usuario = await _context.Usuarios.FindAsync(userId);
            if (usuario == null) return NotFound("Usuario no encontrado");

            var billetera = await _appMp.CrearCuentaMercadoPago(usuario);
            return Ok(billetera);
        }

        [HttpPost("crear/cuentadni")]
        public async Task<IActionResult> CrearCuentaDNI()
        {
            var userId = int.Parse(User.Claims.First(c => c.Type == "id_usuario").Value);
            var usuario = await _context.Usuarios.FindAsync(userId);
            if (usuario == null) return NotFound("Usuario no encontrado");

            var billetera = await _cuentaDni.CrearCuentaDni(usuario);
            return Ok(billetera);
        }


        [HttpPost("crear/paypal")]
        public async Task<IActionResult> CrearCuentaPaypal()
        {
            var userId = int.Parse(User.Claims.First(c => c.Type == "id_usuario").Value);
            var usuario = await _context.Usuarios.FindAsync(userId);
            if (usuario == null) return NotFound("Usuario no encontrado");

            var billetera = await _payPal.CrearCuentaPayPal(usuario);
            return Ok(billetera);
        }


    }
}
