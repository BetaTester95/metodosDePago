using BilleterasBack;
using BilleterasBack.Wallets.Collector.Cobrador;
using BilleterasBack.Wallets.Data;
using BilleterasBack.Wallets.Shared.Interfaces;
using BilleterasBack.Wallets.Shared.Strategies.Pp;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BilleterasBack.Wallets.Shared.Strategies.Mp
{
    public class MpPagoConTransferencia : IPagoCardTransferencia
    {
        private readonly AppDbContext _context;

        public MpPagoConTransferencia(AppDbContext context)
        {
           _context = context;
        }

        public bool PagoConTransferencia(decimal montoPagar, string cbu)
        {
            //agregar ilogger para debuguear
            var logger = LoggerFactory.Create(builder => builder.AddConsole()).CreateLogger<MpPagoConTransferencia>();

            string cvuCobrador = "0046922191583351343977";
            int idDni = 12345678;

            var billeteraCobrador = _context.Billeteras.FirstOrDefault(b => b.Tipo == "Cobrador" && b.Cvu == cbu);
            if(billeteraCobrador == null)
            {
                logger.LogError($"Billetera del cobrador no encontrada. {{Cbu: {cbu}}}");
                return false;
            }
            
            var tarjetaUsuario = _context.Tarjetas.Include(t => t.Billetera)
                 .ThenInclude(b => b.Usuario)
                 .FirstOrDefault(t => t.Billetera.Usuario.Dni == idDni && t.Billetera.Tipo == "MercadoPago");

            if (tarjetaUsuario == null)
            {
                logger.LogError("Tarjeta del usuario no encontrada.");
                return false;
            }

            var billeteraUsuario = _context.Billeteras.Include(u => u.Usuario).FirstOrDefault(b => b.Usuario.Dni == idDni && b.Tipo == "MercadoPago");
            if(billeteraUsuario == null)
            {
                logger.LogError("Billetera del usuario no encontrada.");
                return false;
            }

            if (billeteraCobrador.Cvu == cbu)
            {
                decimal saldoMp = billeteraUsuario.Saldo;

                if (saldoMp < montoPagar)
                {
                    logger.LogWarning($"Saldo insuficiente. Saldo actual: {saldoMp}, monto a pagar: {montoPagar}");
                    return false; 
                }

                saldoMp -= montoPagar;
                billeteraCobrador.Saldo += montoPagar;
                billeteraUsuario.Saldo = saldoMp;
                _context.SaveChanges();
                return true;
            }
            else
            {
               return false;
            }
        }
    }
}