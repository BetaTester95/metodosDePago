using BilleterasBack.Wallets.Collector.Cobrador;
using BilleterasBack.Wallets.Data;
using BilleterasBack.Wallets.Shared.Interfaces;
using BilleterasBack.Wallets.Shared.Strategies.Mp;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BilleterasBack.Wallets.Shared.Strategies.Pp
{
    public class paypPagoConTransferencia : IPagoCardTransferencia
    {
        
        private readonly AppDbContext _context;
        private decimal _saldoPayPal;


        public paypPagoConTransferencia(AppDbContext context)
        {
           _context = context;

        }

        public bool PagoConTransferencia(decimal montoPagar, string cbu)
        {
            var logger = LoggerFactory.Create(builder => builder.AddConsole()).CreateLogger<paypPagoConTransferencia>();

            string mail = cbu;

            int idDni = 12345678;

            var billeteraCobrador = _context.Billeteras.Include(b => b.Usuario).FirstOrDefault(b => b.Tipo == "Cobrador" && b.Usuario.Email == mail);
            if (billeteraCobrador == null) 
            {
                logger.LogError($"Billetera del cobrador no encontrada. {{Mail: {mail}}}"); 
                return false;
            }

            var billeteraUsuario = _context.Billeteras.Include(b => b.Usuario).FirstOrDefault(b => b.Usuario.Dni == idDni && b.Tipo == "PayPal");
            if (billeteraUsuario == null)
            {
                logger.LogError("Billetera del usuario no encontrada.");
                return false;
            }


            if (montoPagar <= 0)
            {
                logger.LogError("El monto a pagar debe ser mayor que cero.");
                return false;
            }

            if (montoPagar > 10000)
            {
                logger.LogError("El monto a pagar excede el límite permitido de 10,000.");
                return false;
            }
           

            if (montoPagar <= 0)
            {
                Console.WriteLine($"No se pudo realizar el pago. ");
                return false;
            }

            if (billeteraUsuario.Saldo < montoPagar)
            {
                logger.LogWarning($"Saldo insuficiente. Saldo actual: {billeteraUsuario.Saldo}, monto a pagar: {montoPagar}");
                return false;
            }

            if (string.IsNullOrEmpty(mail))
            {
                logger.LogError("El mail no puede estar vacío.");
                return false;
            }

            if (mail != billeteraCobrador.Cvu)
            {
                Console.WriteLine($"La cuenta asociada con el mail: {mail} no existe. ");
                return false;
            }

            _saldoPayPal = billeteraUsuario.Saldo;
            _saldoPayPal -= montoPagar;
            billeteraUsuario.Saldo = _saldoPayPal;
            billeteraCobrador.Saldo += montoPagar;
            _context.SaveChanges();
            logger.LogInformation($"Pago realizado exitosamente. {montoPagar} transferido a {mail}");
            return true;
        }


    }
}
