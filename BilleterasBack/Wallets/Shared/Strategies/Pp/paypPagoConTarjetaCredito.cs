using BilleterasBack.Wallets.Collector.Cobrador;
using BilleterasBack.Wallets.Shared.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BilleterasBack.Wallets.Shared.Strategies.Pp
{
    public class paypPagoConTarjetaCredito : IpagoCardCred
    {
      
        private readonly AppDbContext _context;
        private decimal saldoTarjetaCredito;
        private string? _cvuCobradorSeleccionado;
        private int _idDni;

        public paypPagoConTarjetaCredito(AppDbContext context)
        {
            _context = context;
        }


        public bool PagoConTarjetaCredito(decimal montoPagar, int cantCuotas=0)
        {
            var logger = LoggerFactory.Create(builder => builder.AddConsole()).CreateLogger<paypAgregarTarjeta>();

            string cvuCobrador = "0046922191583351343977";
            int idDni = 12345678;

            var billeteraCobrador = _context.Billeteras.Include(b => b.Usuario).FirstOrDefault(b => b.Tipo == "Cobrador" && cvuCobrador == b.Cvu); //revisamos que exista el cobrador
            if(billeteraCobrador == null)
            {
                logger.LogWarning("El cobrador con CVU: {cvuCobrador} no existe.", cvuCobrador);
                return false;
            }

            var tarjetaUsuario = _context.Tarjetas.Include(t => t.Billetera)
                 .ThenInclude(b => b.Usuario)
                 .FirstOrDefault(t => t.Billetera.Usuario.Dni == idDni && t.Billetera.Tipo == "PayPal");

            var billeteraUsuario = _context.Billeteras.Include(u => u.Usuario).FirstOrDefault(b => b.Usuario.Dni == idDni && b.Tipo == "PayPal");
            saldoTarjetaCredito = billeteraCobrador.Saldo;

            if (tarjetaUsuario == null)
            {
                logger.LogWarning("La tarjeta asociada al DNI: {idDni} no existe.", idDni);
                return false;
            }
                       
            if (billeteraUsuario == null)
            {
                logger.LogWarning("La billetera asociada al DNI: {idDni} no existe.", idDni);
                return false;
            }
            
            if (montoPagar <= 0)
            {
                logger.LogWarning("El monto a pagar debe ser mayor que cero.");
                return false;
            }
            if (montoPagar > saldoTarjetaCredito)
            {
                logger.LogWarning("El monto a pagar excede el saldo disponible en la tarjeta de crédito.");
                return false;
            }

            saldoTarjetaCredito -= montoPagar;
            billeteraCobrador.Saldo += montoPagar;
            _context.SaveChanges();
            return true;
        }
    }
}
