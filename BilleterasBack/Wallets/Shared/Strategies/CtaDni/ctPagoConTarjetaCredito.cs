using BilleterasBack.Wallets.Collector.Cobrador;
using BilleterasBack.Wallets.Shared.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EjercicioInterfaces.Estrategias.ctdEstrategias
{
    public class ctPagoConTarjetaCredito : IpagoCardCred
    {

        public TarjetaVirtual? tarjetaV;
        public Cobrador cobrador;
        public CuentaDni accountDni;

        public ctPagoConTarjetaCredito(CuentaDni ctadni,Cobrador collector)
        {
            cobrador = collector;
            accountDni = ctadni;
        }

        public bool PagoConTarjetaCredito(decimal montoPagar, int cantCuotas)
        {
            if (accountDni.tarjetaV == null || accountDni.tarjetaV.numeroTarjetaVirtual == null)
            {
                Console.WriteLine("No hay una tarjeta cargada para poder pagar.");
                return false;
            }

            if (cobrador == null || cobrador.cbu == null)
            {
                Console.WriteLine($"No existe un cobrador asociado a ese cbu. ");
                return false;
            }

            decimal saldoTarjetaCredito = accountDni.tarjetaV.saldo();

            if (saldoTarjetaCredito < montoPagar)
            {
                Console.WriteLine("Saldo insuficiente en la tarjeta de credito virtual.");
                return false;
            }

            var tarjetaVirtual = tarjetaV!.limiteSaldo -= montoPagar;
            accountDni.tarjetaV.limiteSaldo = saldoTarjetaCredito;

            cobrador.cobrarMonto(montoPagar);
            Console.WriteLine($"¡Se realizó el pago!");

            return true;
        }
    }
}
