using BilleterasBack.Wallets.Data;
using BilleterasBack.Wallets.Dtos;
using BilleterasBack.Wallets.Models;
using BilleterasBack.Wallets.Servicios;
using BilleterasBack.Wallets.Validaciones;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;

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


        private class respuestasController
        {
            public bool? Success { get; set; }
            public string? Message { get; set; }
            public object? Data { get; set; }
        }

        [HttpPost("crear/usuario")] //crear
        public async Task<IActionResult> crearUsuario([FromBody] Usuario nuevoUsuario)
        {
       

            var resultado = await _usuarioServicios.crearUsuario(nuevoUsuario);
                if (!resultado.IsSuccess)
                {
                    return Ok(new
                    {
                        success = false,
                        mensaje = resultado.ErrorMessage
                    });
                }      
            return Ok(resultado);       
        }
        //IACRESULT ?? INFO
        [HttpPut("editar/usuario/{id}")] //editar
        public async Task<IActionResult> editarUsuario(int id, [FromBody] UserDto usuarioActualizado)
        {
            var res = new respuestasController { Success = false, Message = "", Data = null };

            try
            {
                var resultado = await _usuarioServicios.editarUsuario(id, usuarioActualizado);
                if (resultado.IsSuccess == false)
                {
                    res.Success = resultado.IsSuccess;
                    res.Message = resultado.ErrorMessage;
                    res.Data = resultado.Data;
                }
                else
                {
                    res.Success = resultado.IsSuccess;
                    res.Message = resultado.ErrorMessage;
                    res.Data = resultado.Data;
                    
                }    
            }
            catch (Exception ex)
            {
                res.Success = false;
                res.Message = "Ocurrió un error: " + ex.Message;
                
            }

            return Ok(res.Data);

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
