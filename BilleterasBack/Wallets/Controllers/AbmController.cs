using Microsoft.AspNetCore.Mvc;
using BilleterasBack.Wallets.Data;
using Microsoft.EntityFrameworkCore;

using BilleterasBack.Wallets.Servicios;

namespace BilleterasBack.Wallets.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AbmController : ControllerBase
    {
        private readonly UsuarioService _usuarioServicios;

        public AbmController(UsuarioService usuarioServicios)
        {
            _usuarioServicios = usuarioServicios;
        }

        [HttpGet("mostrar/usuarios")]
        public async Task<IActionResult> listarUsuarios()
        {
            try
            {
                var usuarios = await _usuarioServicios.mostrarUsuarios();
                if (usuarios == null)
                    return BadRequest(new { mensaje = "No se encontraron usuarios." });

                return Ok(usuarios);
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensaje = "Error al obtener los usuarios.", detalle = ex.Message });
            }
        }

        [HttpGet("test")]
        public IActionResult Test()
        {
            return Ok("Controller funcionando");
        }
    }
}
