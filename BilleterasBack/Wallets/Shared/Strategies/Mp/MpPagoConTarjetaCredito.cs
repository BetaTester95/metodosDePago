using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EjercicioInterfaces.Pagos;
namespace EjercicioInterfaces.Estrategias.MercadoPago
{

    public class MpPagoConTarjetaCredito : IpagoCardCred
    {
        public Cobrador cobrador;
        public AppMp cuenta;
        public MpPagoConTarjetaCredito(Cobrador collector, AppMp mp)
        {
            cobrador = collector;
            cuenta = mp;
        }

        public bool PagoConTarjetaCredito(decimal montoPagar, int cantCuotas)
        {
            if (cuenta.tarjeta!.numeroTarjeta == null)
            {
                Console.WriteLine($"No tenes una tarjeta asociada .");
                return false;
            }
            if (cobrador == null || cobrador.cbu == null)
            {
                Console.WriteLine("No hay un cobrador asociado a ese CBU.");
                return false;
            }

            decimal saldoTarjetaCredito = cuenta.tarjeta.SaldoLimite();
            decimal resultado;

            if (montoPagar <= 0)
            {
                Console.WriteLine("Error el monto es 0");
                return false;
            }

            if (montoPagar > saldoTarjetaCredito)
            {
                Console.WriteLine($"El monto supera su saldo de la tarjeta de credito: ${saldoTarjetaCredito} Pesos");
                return false;
            }
            if (cantCuotas > 12)
            {
                Console.WriteLine($"La cantidad de cuotas que selecciono {cantCuotas} no estan permitidas. ");
                return false;
            }

            if (cantCuotas == 1)
            {
                saldoTarjetaCredito -= montoPagar;
                cobrador.cobrarMonto(montoPagar);
                Console.WriteLine($"En la tarjeta de credito le quedo un saldo de: ${saldoTarjetaCredito} Pesos");
                Console.WriteLine($"\n");
                Console.WriteLine($"DATOS DEL COBRADOR: ");
                Console.WriteLine($"Nombre Completo: {cobrador.nombre} {cobrador.apellido} ");
                Console.WriteLine($"DNI: {cobrador.dni}");
                Console.WriteLine($"Se realizo el pago total de: ${montoPagar} Pesos");
                Console.WriteLine($"\n");

                return true;
            }
            if (cantCuotas == 3)
            {
                resultado = montoPagar + (0.15m * montoPagar);

                saldoTarjetaCredito -= resultado;
                cobrador.cobrarMonto(resultado);
                Console.WriteLine($"\n");
                Console.WriteLine($"DATOS DEL COBRADOR: ");
                Console.WriteLine($"Nombre Completo: {cobrador.nombre} {cobrador.apellido} ");
                Console.WriteLine($"DNI: {cobrador.dni}");
                Console.WriteLine($"Se realizo el pago total de: ${resultado}Pesos");
                Console.WriteLine($"\n");

                return true;
            }

            if (cantCuotas == 6)
            {
                resultado = montoPagar + (0.20m * montoPagar);

                saldoTarjetaCredito -= resultado;
                cobrador.cobrarMonto(resultado);
                Console.WriteLine($"\n");
                Console.WriteLine($"Su saldo de la tarjeta de credito actualmente: ${saldoTarjetaCredito} Pesos");
                Console.WriteLine($"DATOS DEL COBRADOR: ");
                Console.WriteLine($"Nombre Completo: {cobrador.nombre} {cobrador.apellido} ");
                Console.WriteLine($"DNI: {cobrador.dni}");
                Console.WriteLine($"Se realizo el pago total de: ${resultado}Pesos");
                Console.WriteLine($"\n");
                return true;

            }
            if (cantCuotas == 12)
            {
                resultado = montoPagar + (0.25m * montoPagar);

                saldoTarjetaCredito -= resultado;
                cobrador.cobrarMonto(resultado);
                Console.WriteLine($"\n");
                Console.WriteLine($"Su saldo de la tarjeta de credito actualmente: ${saldoTarjetaCredito} Pesos");
                Console.WriteLine($"DATOS DEL COBRADOR: ");
                Console.WriteLine($"Nombre Completo: {cobrador.nombre} {cobrador.apellido} ");
                Console.WriteLine($"DNI: {cobrador.dni}");
                Console.WriteLine($"Se realizo el pago total de: ${resultado}Pesos");
                Console.WriteLine($"\n");
                return true;

            }
            Console.WriteLine($"Error no se pudo pagar. ");
            return false;
        }
        
    }
}
