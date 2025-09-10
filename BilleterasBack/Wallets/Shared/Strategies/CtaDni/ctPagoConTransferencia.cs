using BilleterasBack.Wallets.Collector.Cobrador;
using BilleterasBack.Wallets.Data;
using BilleterasBack.Wallets.Shared.Interfaces;
using BilleterasBack.Wallets.Shared.Strategies.Mp;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace BilleterasBack.Wallets.Shared.Strategies.CtaDni
{
    public class ctPagoConTransferencia : IPagoCardTransferencia
    {
        private readonly AppDbContext _context;
        private string? _cvuCobradorSeleccionado;
        private int _idDni;
        private decimal descuentoTotal;
        private decimal descuentoLyM = 0.15m;
        private decimal descuentoS = 0.20m;
        public ctPagoConTransferencia(AppDbContext context)
        {
          _context = context;
        }

        public bool PagoConTransferencia(decimal montoPagar, string cbu)
        {
            var logger = LoggerFactory.Create(builder => builder.AddConsole()).CreateLogger<ctPagoConTransferencia>();
            int idDni = 12345678;

            var billeteraCobrador = _context.Billeteras.FirstOrDefault(b => b.Tipo == "Cobrador" && b.Cvu == cbu);
            if (billeteraCobrador == null)
            {
                logger.LogError($"Billetera del cobrador no encontrada. {{Cbu: {cbu}}}");
                return false;
            }

            var tarjetaUsuario = _context.Tarjetas.Include(t => t.Billetera)
               .ThenInclude(b => b.Usuario)
               .FirstOrDefault(t => t.Billetera.Usuario.Dni == idDni && t.Billetera.Tipo == "CuentaDni");

            if (tarjetaUsuario == null)
            {
                logger.LogError("Tarjeta del usuario no encontrada.");
                return false;
            }

            var billeteraUsuario = _context.Billeteras.Include(u => u.Usuario).FirstOrDefault(b => b.Usuario.Dni == idDni && b.Tipo == "MercadoPago");
            if (billeteraUsuario == null)
            {
                logger.LogError("Billetera del usuario no encontrada.");
                return false;
            }
            decimal saldo = billeteraUsuario.Saldo;

            DateTime hoy = DateTime.Now;
            var region = new CultureInfo("es-ES");
            string diaSemana = hoy.ToString("dddd", region).ToLower();

            if (saldo < montoPagar)
            {
                logger.LogError("Saldo insuficiente para realizar el pago.");
                return false;
            }

            if (diaSemana == "lunes" || diaSemana == "miércoles")
            {
                descuentoTotal = montoPagar - (descuentoLyM * montoPagar);
                saldo -= descuentoTotal;
                billeteraUsuario.Saldo = saldo;
                billeteraCobrador.Saldo += descuentoTotal;
                _context.SaveChanges();              
                return true;
            }
            else if (diaSemana == "sábado")
            {
                descuentoTotal = montoPagar - (descuentoS * montoPagar);
                saldo -= descuentoTotal;
                billeteraUsuario.Saldo = saldo;
                billeteraCobrador.Saldo += descuentoTotal;
                _context.SaveChanges();
                return true;
            }

            if (montoPagar >= saldo)
            {
                saldo -= montoPagar;
                billeteraUsuario.Saldo = saldo;
                billeteraCobrador.Saldo += montoPagar;
                _context.SaveChanges();
               return true;
           }
           return false;
        }
    }
}
