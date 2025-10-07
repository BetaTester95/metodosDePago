
using BilleterasBack.Wallets.Collector.Cobrador;
using BilleterasBack.Wallets.CuentaDni;
using BilleterasBack.Wallets.Data;
using BilleterasBack.Wallets.PayPal;
using Microsoft.AspNetCore.Mvc;


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
        public async Task<object> CrearMercadoPago(int dni)
        {
            try
            {
                var billetera = await _appMp.CrearCuentaMercadoPago(dni);
                return Ok(billetera);
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        [HttpPost("crear/cuentadni")]
        public async Task<object> CrearCuentaDNI(int dni)
        {
            try 
            {
                var billetera = await _cuentaDni.CrearCuentaDni(dni);
                return billetera;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        

        [HttpPost("crear/paypal")]
        public async Task<object> CrearCuentaPaypal(string email)
        {
            try
            {
                var billetera = await _payPal.CrearCuentaPayPal(email);
                return billetera;
            }
            catch (Exception ex)
            {
                return ex;
            }
        }

        [HttpPost("crear/cobrador")]
        public async Task<IActionResult> CrearCobrador(int dni)
        {
            var usuario = await _cobrador.CrearCuentaCobrador(dni);
                if(!usuario.Success)
                {
                    return Ok(new
                    {   
                        success = true,
                        message = usuario.Message
                    });
                }
                return Ok(new
                {
                    message = "Billetera de Cobrador creada exitosamente.",
                    datos = usuario.Data
                });       
        }


        [HttpPost("cargar/mercadopago")]
        public async Task<object> cargarSaldoMp(int dni, decimal monto)
        {
            try
            {
                var resultado = await _appMp.agregarDineroCuentaMp(dni, monto);
                return Ok(new { Success = resultado ? resultado : false, _appMp.Message });
            }
            catch (Exception ex)
            {
                return ex.Message;

            }
        }

        [HttpPost("cargar/ctadni")]
        public async Task<object> cargarSaldoCtaDni(int dni, decimal monto)
        {
              try
              {
                  var resultado = await _cuentaDni.CargarSaldoCtaDni(dni, monto);
                  return Ok(new { Success = resultado ? resultado : false, _cuentaDni.Message });
              }
              catch (Exception ex)
              {
                  return ex.Message;
              }
        }

        [HttpPost("cargar/paypal")]
        public async Task<object> cargarSaldoPaypal(int dni, decimal monto)
        {
            try
            {
                var resultado = await _payPal.AgregarSaldoPaypal(dni, monto);
                return Ok(new { Success = resultado ? resultado : false, _payPal.Message });
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }


    }
}
