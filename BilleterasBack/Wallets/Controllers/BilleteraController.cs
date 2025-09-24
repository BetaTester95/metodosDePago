
using BilleterasBack.Wallets.Collector.Cobrador;
using BilleterasBack.Wallets.CuentaDni;
using BilleterasBack.Wallets.Data;
using BilleterasBack.Wallets.Models;
using BilleterasBack.Wallets.PayPal;
using BilleterasBack.Wallets.Collector;
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
        private readonly CuentaDniServicio _cuentaDni;
        private readonly PayPalServicio _payPal;
        private readonly Cobrador _cobrador;
        public BilleteraController(AppDbContext context)
        {
            _context = context;
            _appMp = new AppMp(_context);
            _cuentaDni = new CuentaDniServicio(_context);
            _payPal = new PayPalServicio(_context);
            _cobrador = new Cobrador(_context);
        }

        [HttpPost("crear/mercadopago")]
        public async Task<IActionResult> CrearMercadoPago(int dni)
        {
                var billetera = await _appMp.CrearCuentaMercadoPago(dni);
                if (!billetera.IsSuccess)
                {
                    return Ok(new
                    {
                        success = false,
                        message = billetera.ErrorMessage
                    });
                }
               return Ok(new
                {
                    message = "Billetera de MercadoPago creada exitosamente.",
                    datos = billetera.Data
                });               
        }

        [HttpPost("crear/cuentadni")]
        public async Task<IActionResult> CrearCuentaDNI(int dni)
        {
            var billetera = await _cuentaDni.CrearCuentaDni(dni);

                if (!billetera.IsSuccess)
                {
                    return Ok(new
                    {
                        message = billetera.ErrorMessage,
                        datos = billetera.Data
                    });
                }
                return Ok(new
                {
                    message = "Billetera de CuentaDni creada exitosamente.",
                    datos = billetera.Data,
                });
        }
        

        [HttpPost("crear/paypal")]
        public async Task<IActionResult> CrearCuentaPaypal(string email)
        {
            var billetera = await _payPal.CrearCuentaPayPal(email);

            if (!billetera.IsSuccess)
            {
                return Ok(new
                {
                    success = true,
                    message = billetera.ErrorMessage
                });
            }
            return Ok(new
            {
                message = "Billetera de PayPal creada exitosamente.",
                datos = billetera.Data
            });
        }

        [HttpPost("crear/cobrador")]
        public async Task<IActionResult> CrearCobrador(int dni)
        {
            var usuario = await _cobrador.CrearCuentaCobrador(dni);
                if(!usuario.IsSuccess)
                {
                    return Ok(new
                    {   
                        success = true,
                        message = usuario.ErrorMessage
                    });
                }
                return Ok(new
                {
                    message = "Billetera de Cobrador creada exitosamente.",
                    datos = usuario.Data
                });       
        }
    }
}
