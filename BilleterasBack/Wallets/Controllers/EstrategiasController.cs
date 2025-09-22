using Azure.Core;
using BilleterasBack.Wallets.Data;
using BilleterasBack.Wallets.Dtos;
using BilleterasBack.Wallets.Exceptions;
using BilleterasBack.Wallets.Models;
using BilleterasBack.Wallets.Shared;
using BilleterasBack.Wallets.Shared.Interfaces;
using BilleterasBack.Wallets.Shared.Strategies.CtaDni;
using BilleterasBack.Wallets.Shared.Strategies.Mp;
using BilleterasBack.Wallets.Shared.Strategies.Pp;
using BilleterasBack.Wallets.Validaciones;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;
using System.Security.Claims;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace BilleterasBack.Wallets.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EstrategiasController : ControllerBase
    {
        private readonly AppDbContext _context;
        public readonly TipoMetodoPago _tipoDePago;
        private readonly Validador _validaciones;

        public EstrategiasController(AppDbContext context, Validador validador)
        {
            _context = context;
            _validaciones = validador;
        }

        [HttpPost("agregar-tarjeta")]
        public async Task<IActionResult> AgregarCard(TipoMetodoPago tipoMetodoPago, [FromBody] TarjetaDTO request)
        {
            try
            {
                var tipoMetodoPagoRecibido = tipoMetodoPago;
                var tipoMetodoPagoStr = tipoMetodoPago.ToString();

            DateTime fechaHoy = DateTime.Now;

            if (string.IsNullOrWhiteSpace(request.Nombre))
                return BadRequest(new { mensaje = "El nombre no puede estar vacío." });

            if (string.IsNullOrWhiteSpace(request.Apellido))
                return BadRequest(new { mensaje = "El apellido no puede estar vacío." });

            if (request.Dni <= 0)
                return BadRequest(new { mensaje = "El DNI debe ser un numero positivo." });

            if (fechaHoy > request.FechaExp)
                return BadRequest(new { mensaje = "La fecha de expiracion no puede ser en el pasado." });

            if (request.Cod < 100 || request.Cod > 999)
                return BadRequest(new { mensaje = "El codigo de seguridad debe tener 3 digitos." });

            if (string.IsNullOrWhiteSpace(request.NumeroTarjeta) || request.NumeroTarjeta.Length > 22 | !request.NumeroTarjeta.All(char.IsDigit))
                return BadRequest(new { mensaje = "El numero de tarjeta debe tener 22 digitos." });

            if (request.NumeroTarjeta.Length < 16)
                return BadRequest(new { mensaje = "El numero de tarjeta debe tener al menos 16 digitos." });

            var existeNumTarjeta = await _context.Tarjetas.AnyAsync(t => t.NumeroTarjeta == request.NumeroTarjeta);
            if (existeNumTarjeta)
                return BadRequest(new { mensaje = "El numero de tarjeta ya esta agregada en su cuenta." });

            var billetera = await _context.Billeteras
                .Include(b => b.Usuario)
                .FirstOrDefaultAsync(b => b.Usuario.Dni == request.Dni && b.Tipo == tipoMetodoPago.ToString());

            bool billeteraEncontrada = billetera != null;


            if (billetera == null)
            {
                System.Diagnostics.Debug.WriteLine("DEBUG: Billetera no encontrada");
                return NotFound(new { mensaje = "Billetera no encontrada para el usuario y tipo especificado" });
            }

            IAgregarCard estrategia = tipoMetodoPago switch
            {
                TipoMetodoPago.MercadoPago => new MpAgregarTarjeta(_context, _logger),
                TipoMetodoPago.CuentaDni => new ctAgregarTarjeta(_context),
                TipoMetodoPago.PayPal => new paypAgregarTarjeta(_context),
                _ => throw new NotImplementedException($"Estrategia no implementada para el tipo de pago: {tipoMetodoPago}")
            };
                _ => throw new NotImplementedException($"Estrategia no implementada para el tipo de pago: {tipoMetodoPago}")
            };
                _ => throw new NotImplementedException($"Estrategia no implementada para el tipo de pago: {tipoMetodoPago}")
            };
                _ => throw new NotImplementedException($"Estrategia no implementada para el tipo de pago: {tipoMetodoPago}")
            };

                var resultado = estrategia.AgregarTarjeta(
                    request.NumeroTarjeta,
                    request.Nombre,
                    request.Apellido,
                    request.Dni,
                    request.FechaExp,
                    request.Cod
                );
             
                return Ok(resultado);
            }
            catch(ArgumentException ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
            catch(UsuarioExceptions ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }

            catch(BilleteraExceptions ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }

            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = "Ocurrió un error inesperado en el servidor." });
            }
        }

        [HttpPost("pagarcontarjeta")]
        public async Task<IActionResult> PagarConTarjeta([FromBody] PagoTarjetaRequest request)
        {
            var logger = HttpContext.RequestServices.GetRequiredService<ILogger<MpPagoConTarjetaCredito>>();
            var logger2 = HttpContext.RequestServices.GetRequiredService<ILogger<ctPagoConTarjetaCredito>>();

            if(_validaciones.ValidarMonto(request.montoPagar))
                return BadRequest(new { mensaje = "Error al ingresar el monto." });
            
            IpagoCardCred? estrategia = request.tipoMetodoPago switch
            {
                TipoMetodoPago.MercadoPago => new MpPagoConTarjetaCredito(_context, logger),
                TipoMetodoPago.CuentaDni => new ctPagoConTarjetaCredito(_context, logger2),
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
            if(_validaciones.ValidarTipoMetodoPago(TipoMetodoPago.ToString()))
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