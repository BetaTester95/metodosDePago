using BilleterasBack.Wallets.Models;
using BilleterasBack.Wallets.Shared.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace BilleterasBack.Wallets.Shared.Strategies.Mp
{
    public class MpAgregarTarjeta : IAgregarCard
    {
        private readonly AppDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public MpAgregarTarjeta(AppDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;

        }
        public bool AgregarTarjeta(string numTarjeta, string nombre, string apellido, int dni, DateTime fechaVenc, int cod)
        {
           try
           {
                var httpContext = _httpContextAccessor.HttpContext;
                var idUsuarioStr = httpContext?.User.Claims.FirstOrDefault(c => c.Type == "idUsuario")?.Value;

                if (string.IsNullOrEmpty(idUsuarioStr))
                    throw new InvalidOperationException("JWT no contiene id_usuario");

                int idUsuario = int.Parse(idUsuarioStr);

                var billetera = _context.Billeteras
                    .Include(b => b.Usuario)
                    .FirstOrDefault(b => b.Usuario.IdUsuario == idUsuario && b.Tipo == "MercadoPago");  

                if (billetera == null)
                    throw new InvalidOperationException("Billetera no encontrada");

                /*
                  // Obtener la billetera del usuario
            var billetera = _context.Billeteras.FirstOrDefault(b => b.Usuario.IdUsuario == idUsuario);
            if (billetera == null)
                throw new InvalidOperationException("No se encontró la billetera del usuario.");

            // Verificar si ya existe la tarjeta para esa billetera
            var tarjetaExistente = _context.Tarjetas
                .FirstOrDefault(t => t.NumeroTarjeta == numTarjeta && t.IdBilletera == billetera.IdBilletera);

            if (tarjetaExistente != null)
                return false; // no duplicar
                 */

                var tarjeta = new Tarjeta
                {
                    NumeroTarjeta = numTarjeta,
                    FechaVencimiento = fechaVenc,
                    CodigoSeguridad = cod,
                    Saldo = 10000,
                };

                _context.Tarjetas.Add(tarjeta);
                _context.SaveChanges();
                return true;

            }

            catch
            {
                    return false;
            }

        }
    }
}