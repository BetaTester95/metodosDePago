using BilleterasBack.Wallets.Cards;
using BilleterasBack.Wallets.Models;
using BilleterasBack.Wallets.Shared;
using BilleterasBack.Wallets.Shared.Interfaces;
using BilleterasBack.Wallets.Shared.Strategies.Mp;
using EjercicioInterfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace BilleterasBack.Wallets.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EstrategiasController : ControllerBase
    {
        private readonly ContextoPago _contextoPago; 
        private readonly AppDbContext _context;

        public EstrategiasController(ContextoPago metodoPago, AppDbContext context)
        {
            _contextoPago = metodoPago; 
            _context = context;
        }

        [Authorize]
        [HttpPost("agregar-tarjeta")]
        public IActionResult AgregarTarjeta([FromBody] Tarjeta tarjeta)
        {
            try
            {
                // 1️⃣ Verificar que llegue la tarjeta
                if (tarjeta == null)
                    return BadRequest(new
                    {
                        error = "Tarjeta es null",
                        paso = "1_validacion_entrada"
                    });

                // 2️⃣ Obtener idUsuario desde el JWT
                var claimUsuario = User.FindFirst("id_usuario")?.Value;

                if (string.IsNullOrEmpty(claimUsuario))
                    return BadRequest(new
                    {
                        error = "Token JWT no contiene id_usuario",
                        paso = "2_validacion_jwt",
                        claim_encontrado = claimUsuario
                    });

                int idUsuario = int.Parse(claimUsuario);

                // 3️⃣ Validar que el usuario exista
                var usuario = _context.Usuarios.FirstOrDefault(u => u.id_usuario == idUsuario && u.activo);

                if (usuario == null)
                    return BadRequest(new
                    {
                        error = "Usuario no encontrado o inactivo",
                        paso = "3_validacion_usuario",
                        id_buscado = idUsuario,
                        usuario_existe = _context.Usuarios.Any(u => u.id_usuario == idUsuario),
                        usuario_activo = _context.Usuarios.Any(u => u.id_usuario == idUsuario && u.activo)
                    });

                // 4️⃣ Asignar datos de usuario y fecha
                tarjeta.id_usuario = idUsuario;
                tarjeta.fecha_creacion = DateTime.Now;
                tarjeta.activo = true;

                // 5️⃣ Debug antes de crear estrategia
                if (_contextoPago == null)
                    return BadRequest(new
                    {
                        error = "ContextoPago es null",
                        paso = "4_validacion_contexto"
                    });

                // 6️⃣ Crear la estrategia
                IAgregarCard estrategia = new MpAgregarTarjeta(_context);
                _contextoPago.CambiarEstrategia(estrategia);

                if (!int.TryParse(tarjeta.dniTitular.ToString(), out int dniInt))
                {
                    return BadRequest(new
                    {
                        error = "DNI inválido",
                        paso = "conversion_dni",
                        dni_recibido = tarjeta.dniTitular
                    });
                }

                // 7️⃣ Procesar la tarjeta
                bool exito = _contextoPago.Procesar(estrategia,
                    tarjeta.numeroTarjeta,
                    tarjeta.nombreTitular,
                    tarjeta.apellidoTitular,
                    tarjeta.dniTitular,
                    tarjeta.fechaVencimiento,
                    tarjeta.cod
                );

                if (!exito)
                    return BadRequest(new
                    {
                        error = "No se pudo agregar la tarjeta",
                        paso = "5_procesamiento_estrategia",
                        tarjeta_datos = new
                        {
                            numero = tarjeta.numeroTarjeta,
                            titular = tarjeta.nombreTitular,
                            dni = tarjeta.dniTitular,
                            id_usuario = tarjeta.id_usuario
                        }
                    });

                return Ok(new
                {
                    mensaje = "Tarjeta agregada correctamente",
                    tarjeta_id = tarjeta.id_tarjeta // si tiene un ID generado
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    error = "Error interno",
                    paso = "excepcion",
                    detalle = ex.Message,
                    stack_trace = ex.StackTrace // Solo para debug, quitar en producción
                });
            }
        }
    }
}