using BilleterasBack.Wallets.Data;
using BilleterasBack.Wallets.Dtos;
using BilleterasBack.Wallets.Shared;
using BilleterasBack.Wallets.Shared.Interfaces;
using BilleterasBack.Wallets.Shared.Strategies.CtaDni;
using BilleterasBack.Wallets.Shared.Strategies.Mp;
using BilleterasBack.Wallets.Shared.Strategies.Pp;
using BilleterasBack.Wallets.Validaciones;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;


namespace BilleterasBack.Wallets.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EstrategiasController : ControllerBase
    {
        private readonly AppDbContext _context;
        public readonly TipoMetodoPago _tipoDePago;
        private readonly Validador? _validaciones;

        public EstrategiasController(AppDbContext context, Validador validador)
        {
            _context = context;
            _validaciones = validador;
        }

        [HttpPost("agregar-tarjeta")]
        public async Task<object> AgregarCard(TipoMetodoPago tipoMetodoPago, [FromBody] TarjetaDTO request)
        {
            try
            {
                var obb = request.fechaVenc;

                IAgregarCard estrategia = tipoMetodoPago switch
                {
                    TipoMetodoPago.MercadoPago => new MpAgregarTarjeta(_context, _validaciones!),
                    TipoMetodoPago.CuentaDni => new ctAgregarTarjeta(_context, _validaciones!),
                    TipoMetodoPago.PayPal => new paypAgregarTarjeta(_context, _validaciones),
                    _ => throw new NotImplementedException($"Estrategia no implementada para el tipo de pago: {tipoMetodoPago}")
                };

                var resultado = estrategia.AgregarTarjeta(
                    request.NumeroTarjeta,
                    request.Nombre,
                    request.Apellido,
                    request.Dni,
                    request.fechaVenc,
                    request.Cod
                );

                return Ok(new { Success = resultado ? resultado :  false, estrategia.Message });
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        [HttpPost("pagarcontarjeta")]
        public async Task<IActionResult> PagarConTarjeta([FromBody] PagoTarjetaRequest request)
        {

            if(_validaciones.ValidarMonto(request.montoPagar))
                return BadRequest(new { mensaje = "Error al ingresar el monto." });
            
            IpagoCardCred? estrategia = request.tipoMetodoPago switch
            {
                TipoMetodoPago.MercadoPago => new MpPagoConTarjetaCredito(_context),
                TipoMetodoPago.CuentaDni => new ctPagoConTarjetaCredito(_context),
                TipoMetodoPago.PayPal => new paypPagoConTarjetaCredito(_context),
                _ => null
            };

            var exito = estrategia?.PagoConTarjetaCredito(request.montoPagar, request.cantCuotas) ?? false;
            if (exito)
            {
                return Ok(new { mensaje = "Pago realizado con tarjeta exitosamente." });
            }
            else
            {
                return BadRequest(new { mensaje = "Error al procesar el pago con tarjeta." });
            }
        }

        [HttpPost("pagarcontransferencia")]
        public async Task<IActionResult> PagoConTransferencia(TipoMetodoPago TipoMetodoPago, decimal montoPagar, string cbu)
        {
            if(!_validaciones.ValidarTipoMetodoPago(TipoMetodoPago.ToString()))
                return BadRequest(new { mensaje = "El tipo de metodo de pago no es valido." });
            
            if (!_validaciones.ValidarMonto(montoPagar))
                return BadRequest(new { mensaje = "El monto a pagar debe ser mayor que cero." });

            if(!_validaciones.validarNumTarjeta(cbu))
                return BadRequest(new { mensaje = "El CBU ingresado no es valido." });

            IPagoCardTransferencia? estrategia = TipoMetodoPago switch
            {
                TipoMetodoPago.MercadoPago => new MpPagoConTransferencia(_context),
                TipoMetodoPago.CuentaDni => new ctPagoConTransferencia(_context),
                TipoMetodoPago.PayPal => new paypPagoConTransferencia(_context),
                _ => null
            };
            var exito = estrategia?.PagoConTransferencia(montoPagar, cbu) ?? false;

            if (!exito)
            {
                return BadRequest(new { mensaje = "Error al procesar el pago con transferencia." });
            }
            else
            {
                return Ok(new { mensaje = "Pago con transferencia exitoso." });
            }
        }
    }
}