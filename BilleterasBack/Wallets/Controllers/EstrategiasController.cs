using BilleterasBack.Wallets.Dtos;
using BilleterasBack.Wallets.Models;
using BilleterasBack.Wallets.Shared;
using BilleterasBack.Wallets.Shared.Interfaces;
using BilleterasBack.Wallets.Shared.Strategies.Mp;
using EjercicioInterfaces;
using EjercicioInterfaces.Estrategias.ctdEstrategias;
using EjercicioInterfaces.Estrategias.ppEstrategias;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
        private readonly ILogger<MpAgregarTarjeta> _logger;
        public EstrategiasController(AppDbContext context, ILogger<MpAgregarTarjeta> logger)
        {
            _context = context;
            _logger = logger;
        }
        
        [HttpPost("agregar-tarjeta")]
        public async Task<IActionResult> AgregarCard(TipoMetodoPago tipoMetodoPago,[FromBody] TarjetaDTO request)
        {
            DateTime fechaHoy = DateTime.Now;

            if (string.IsNullOrWhiteSpace(request.Nombre))     
                return BadRequest(new { mensaje = "El nombre no puede estar vacío." });

            if (string.IsNullOrWhiteSpace(request.Apellido))
                return BadRequest(new { mensaje = "El apellido no puede estar vacío." });

            if (request.Dni <= 0)
                return BadRequest(new { mensaje = "El DNI debe ser un numero positivo." });

            if(fechaHoy > request.FechaExp)
                return BadRequest(new { mensaje = "La fecha de expiracion no puede ser en el pasado." });

            if(request.Cod < 100 || request.Cod > 999)
                return BadRequest(new { mensaje = "El codigo de seguridad debe tener 3 digitos." });

            if (string.IsNullOrWhiteSpace(request.NumeroTarjeta) || request.NumeroTarjeta.Length != 16 || !request.NumeroTarjeta.All(char.IsDigit))
                return BadRequest(new { mensaje = "El numero de tarjeta debe tener 16 digitos." });

            var existeNumTarjeta = await _context.Tarjetas.AnyAsync(t => t.NumeroTarjeta == request.NumeroTarjeta);
            if (existeNumTarjeta)
                return BadRequest(new { mensaje = "El numero de tarjeta ya esta agregada en su cuenta." });
           
            var billetera = await _context.Billeteras
                .Include(b => b.Usuario)
                .FirstOrDefaultAsync(b => b.Usuario.Dni == request.Dni && b.Tipo == tipoMetodoPago.ToString());

            if (billetera == null)
            {
                System.Diagnostics.Debug.WriteLine("DEBUG: Billetera no encontrada");
                return NotFound(new { mensaje = "Billetera no encontrada para el usuario y tipo especificado" });
            }
               
            IAgregarCard estrategia = tipoMetodoPago switch
            {
               TipoMetodoPago.MercadoPago => new MpAgregarTarjeta(_context, _logger),
               TipoMetodoPago.CuentaDNI => new ctAgregarTarjeta(_context),
               TipoMetodoPago.PayPal => new paypAgregarTarjeta(_context),
               _=> throw new NotImplementedException($"Estrategia no implementada para el tipo de pago: {tipoMetodoPago}")
            };

            var resultado = estrategia.AgregarTarjeta(
                request.NumeroTarjeta,
                request.Nombre,
                request.Apellido,
                request.Dni,
                request.FechaExp,
                request.Cod
            );
            var responseObj = new
            {
                mensaje = resultado ? "Tarjeta agregada correctamente" : "Error al agregar tarjeta",
                datos = new
                {
                    request.NumeroTarjeta,
                    request.Nombre,
                    request.Apellido,
                    request.Dni,
                    request.FechaExp,
                    request.Cod,
                    TipoMetodoPago = tipoMetodoPago.ToString()
                },
                resultadoEstrategia = resultado
            };
            return Ok(responseObj);
        }
    }
}