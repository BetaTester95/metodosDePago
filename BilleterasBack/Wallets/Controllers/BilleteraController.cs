using BilleterasBack.Wallets.Collector.Cobrador;
using BilleterasBack.Wallets.Models;
using EjercicioInterfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BilleterasBack.Wallets.Controllers
{


    [ApiController]
    [Route("api/[controller]")]
    public class BilleteraController : ControllerBase
    {

        private readonly AppDbContext _context;
        private readonly AppMp _appMp;
        private readonly CuentaDni _cuentaDni;
        private readonly PayPal _payPal;
        private readonly Cobrador _cobrador;
        public BilleteraController(AppDbContext context)
        {
            _context = context;
            _appMp = new AppMp(_context);
            _cuentaDni = new CuentaDni(_context);
            _payPal = new PayPal(_context);
            _cobrador = new Cobrador(_context);
        }

        [HttpPost("crear/mercadopago")]
        public async Task<IActionResult> CrearMercadoPago(int dni)
        {
            try
            {
                var billetera = await _appMp.CrearCuentaMercadoPago(dni);
                return Ok(new
                {
                    mensaje = "Billetera de MercadoPago creada exitosamente.",
                    datos = new
                    {
                        idBilletera = billetera.IdBilletera,
                        idUsuario = billetera.IdUsuario,
                        tipo = billetera.Tipo,
                        cvu = billetera.Cvu,
                        saldo = billetera.Saldo
                    }
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    mensaje = ex.Message,
                    datos = (object?)null
                });
            }
        }

        [HttpPost("crear/cuentadni")]
        public async Task<IActionResult> CrearCuentaDNI(int dni)
        {
            try
            {
                var billetera = await _cuentaDni.CrearCuentaDni(dni);
                return Ok(new
                {
                    mensaje = "Billetera de CuentaDni creada exitosamente.",
                    datos = billetera
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    mensaje = "Error al crear la billetera.",
                    detalle = ex.Message
                });
            }
        }

        [HttpPost("crear/paypal")]
        public async Task<IActionResult> CrearCuentaPaypal(int dni)
        {
            try
            {
                var billetera = await _payPal.CrearCuentaPayPal(dni);

                if (billetera == null)
                    return NotFound("Billetera de PayPal no encontrada.");

                return Ok(new
                {
                    mensaje = "Billetera de PayPal creada exitosamente.",
                    datos = billetera
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    mensaje = "Error al crear la billetera.",
                    detalle = ex.Message
                });
            }
        }

        [HttpPost("crear/cobrador")]
        public async Task<IActionResult> CrearCobrador(int dni)
        {
            try
            {
                var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.Dni == dni);
                if (usuario == null) return NotFound("Usuario no encontrado");
                var billetera = await _cobrador.CrearCuentaCobrador(usuario);

                return Ok(new
                {
                    mensaje = "Billetera de Cobrador creada exitosamente.",
                    datos = billetera
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    mensaje = "Error al crear la billetera.",
                    detalle = ex.Message
                });
            }
        }
    }
}
