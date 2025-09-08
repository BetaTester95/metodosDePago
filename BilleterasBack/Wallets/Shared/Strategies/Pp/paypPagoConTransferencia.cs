using BilleterasBack.Wallets.Collector.Cobrador;
using BilleterasBack.Wallets.Shared.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EjercicioInterfaces.Estrategias.ppEstrategias
{
    public class paypPagoConTransferencia : IPagoCardTransferencia
    {
        
        private readonly AppDbContext _context;

        public paypPagoConTransferencia(AppDbContext context)
        {
           _context = context;

        }

        public bool PagoConTransferencia(decimal montoPagar, string cbu)
        {
            string mail = cbu;

            if (montoPagar == 0)
            {
                Console.WriteLine($"Error no se puede transferir 0 USD. ");
                return false;
            }

            //if (ppal.saldoPayPal <= 0)
            //{
            //    Console.WriteLine($"Saldo insuficiente para trasferir. ");
            //    return false;
            //}



            if (montoPagar <= 0)
            {
                Console.WriteLine($"No se pudo realizar el pago. ");
                return false;
            }

            //if (ppal.saldoPayPal < montoPagar)
            //{
            //    Console.WriteLine($"Saldo insuficiente. ");
            //    return false;
            //}

            if (string.IsNullOrEmpty(mail))
            {
                Console.WriteLine($"error mail vacio.");

                return false;

            }

            if (mail != cobrador.mailCobrador)
            {
                Console.WriteLine($"La cuenta asociada con el mail: {mail} no existe. ");
                return false;
            }

            //ppal.saldoPayPal -= montoPagar;
            //cobrador.cobrarMonto(montoPagar);
            //Console.WriteLine($"\n");
            //Console.WriteLine($"Se realizo el pago");
            //Console.WriteLine($"Se realizo una transferencia de: {montoPagar}");
            //Console.WriteLine($"Pago realizado al mail: {mail}");
            //Console.WriteLine($"Su saldo actual es de: ${ppal.saldoPayPal} USD");
            //Console.WriteLine($"\n");

            return true;
        }


    }
}
