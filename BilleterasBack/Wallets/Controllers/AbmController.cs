using BilleterasBack.Wallets.Data;
using BilleterasBack.Wallets.Dtos;
using BilleterasBack.Wallets.Models;
using BilleterasBack.Wallets.Servicios;
using BilleterasBack.Wallets.Validaciones;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using System.Reflection.Metadata.Ecma335;

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

        //[HttpPost("crear/usuario")] //crear
        //public async Task<Usuario> CrearUsuario([FromBody] Usuario nuevoUsuario)
        //{
        //    try
        //    {
        //        var users = await _usuarioServicios.CrearUsuario(nuevoUsuario);
        //        return users!;
        //    }

        //    catch (Exception) 
        //    {
        //        return null;
        //    }
        //}

        //IACRESULT ?? INFO
        [HttpPut("editar/usuario/{id}")] //editar
        public async Task<UserDto> editarUsuario(int id, [FromBody] UserDto usuarioActualizado)
        {
            try
            {
                var usuario = await _usuarioServicios.editarUsuario(id, usuarioActualizado);
                return usuario;
            }
            catch (Exception ex)
            {
                return new UserDto { Message = ex.Message };
            }          
        }

        [HttpGet("mostrar/usuarios")]
        public async Task<List<Usuario>> listarUsuarios()
        {
            try
            {
                var usuarios = await _usuarioServicios.mostrarUsuarios();
                return usuarios;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

       [HttpDelete("eliminar/usuario/{id}")]
       public async Task<object> eliminarUsuario(int id)
       {
           try
           {
               var resultado = await _usuarioServicios.borrarUsuario(id);
               return new
               {
                   Message = "Usuario eliminado exitosamente. "
               };
           }

           catch (Exception ex)
           {
                return ex;
           }
       }
    }
}
