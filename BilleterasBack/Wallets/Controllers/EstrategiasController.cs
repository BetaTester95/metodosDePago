using BilleterasBack.Wallets.Models;
using BilleterasBack.Wallets.Shared;
using BilleterasBack.Wallets.Shared.Interfaces;
using BilleterasBack.Wallets.Shared.Strategies.Mp;
using BilleterasBack.Wallets.Dtos;
using EjercicioInterfaces;
using EjercicioInterfaces.Estrategias.ctdEstrategias;
using EjercicioInterfaces.Estrategias.ppEstrategias;
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
        private readonly AppDbContext _context;
        private readonly ContextoPago _contextoPago;
        public readonly TipoMetodoPago _tipoUsuario;
        public EstrategiasController(AppDbContext context)
        {
            _context = context;
            _contextoPago = new ContextoPago(TipoMetodoPago.MercadoPago, new MpAgregarTarjeta(context));
        }

        [HttpPost("agregar-tarjeta")]
        public IActionResult AgregarCard([FromBody]TarjetaDTO request)
        {

        }
    }
}