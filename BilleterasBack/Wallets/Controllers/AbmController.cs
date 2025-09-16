using Microsoft.AspNetCore.Mvc;
using BilleterasBack.Wallets.Data;
using Microsoft.EntityFrameworkCore;

using BilleterasBack.Wallets.Servicios;
using BilleterasBack.Wallets.Models;
using BilleterasBack.Wallets.Dtos;

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


        [HttpPost("crear/usuario")] //crear
        public async Task<IActionResult> crearUsuario([FromBody] Usuario nuevoUsuario)
        {
            try
            {
                var resultado = await _usuarioServicios.crearUsuario(nuevoUsuario);
                if (resultado == null)
                    return BadRequest(new { mensaje = "Error al crear el usuario." });

                // Extraer el mensaje
                var mensaje = resultado.GetType().GetProperty("mensaje")?.GetValue(resultado)?.ToString();

                // Si contiene "ya existe", es un error de conflicto
                if (mensaje != null && mensaje.Contains("ya existe"))
                {
                    return Conflict(resultado); // HTTP 409 - Angular lo recibe en el bloque error
                }
                return Ok(resultado);
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensaje = "Error al crear el usuario.", msg = ex.Message });
            }
        }

        [HttpPut("editar/usuario/{id}")] //editar
        public async Task<IActionResult> editarUsuario(int id, [FromBody] UserDto usuarioActualizado)
        {
            try
            {
                var resultado = await _usuarioServicios.editarUsuario(id, usuarioActualizado);
                if (resultado == null)
                    return BadRequest(new { mensaje = "Error al actualizar el usuario." });

                // Solución: Usar reflexión para obtener la propiedad "error" del objeto resultado
                var errorProp = resultado.GetType().GetProperty("error");
                var errorValue = errorProp?.GetValue(resultado)?.ToString();

                if (errorValue == "email" || errorValue == "dni")
                    return Conflict(resultado); // 409

                return Ok(resultado);
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensaje = "Error al actualizar el usuario.", msg = ex.Message });
            }
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
                return BadRequest(new { mensaje = "Error al obtener los usuarios.", msg = ex.Message });
            }
        }

       [HttpDelete("eliminar/usuario/{id}")]
       public async Task<IActionResult> eliminarUsuario(int id)
       {
           try
           {
               var resultado = await _usuarioServicios.borrarUsuario(id);
               if (resultado == null)
                   return BadRequest(new { mensaje = "Error al eliminar el usuario." });
               return Ok(resultado);
           }
           catch (Exception ex)
           {
               return BadRequest(new { mensaje = "Error al eliminar el usuario.", msg = ex.Message });
           }
       }
    }
}
