using EjercicioInterfaces.Pagos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EjercicioInterfaces.Estrategias.ppEstrategias
{
    public class paypPagoConTarjetaCredito : IpagoCardCred
    {
        public Tarjeta? tarjeta;
        public Cobrador cobrador;
        public PayPal paypal;

        public paypPagoConTarjetaCredito(PayPal ppal, Cobrador collector)
        {
            paypal = ppal;
            cobrador = collector;
            tarjeta = ppal.tarjeta; // Agregar esta línea
        }

        public bool PagoConTarjetaCredito(decimal montoPagar, int cantCuotas)
        {
            decimal saldoTarjetaCredito = paypal.tarjeta!.limiteSaldo;
            if (paypal.tarjeta.numeroTarjeta == null || paypal.tarjeta.numeroTarjeta == "")
            {
                Console.WriteLine($"\n");
                Console.WriteLine("No hay tarjetas asociadas a esta cuenta PayPal");
                Console.WriteLine("Debe agregar una tarjeta para realizar esta operación. ");
                return false;
            }

            if (string.IsNullOrEmpty(cobrador.cbu))
            {
                Console.WriteLine($"No hay un cobrador asociado");
                return false;
            }

            if (montoPagar <= 0)
            {
                Console.WriteLine($"\n");
                Console.WriteLine("Error al pagar. ");
                return false;
            }

            saldoTarjetaCredito -= montoPagar;
            cobrador.cobrarMonto(montoPagar);

            decimal mostrarSaldo = cobrador.retornarSaldo();
            Console.WriteLine($"===== PAGO REALIZADO EXITOSAMENTE ====");
            Console.WriteLine("\n");
            Console.WriteLine($"Se realizo el pago correctamente. ");
            Console.WriteLine($"Saldo actual: ${paypal.tarjeta.limiteSaldo} USD");
            Console.WriteLine("\n");
            Console.WriteLine($"===== PAGO REALIZADO EXITOSAMENTE ====");

            return true;
        }
        

    }
}
