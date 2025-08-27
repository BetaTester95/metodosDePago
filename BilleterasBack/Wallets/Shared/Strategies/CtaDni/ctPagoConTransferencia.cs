using EjercicioInterfaces.Pagos;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EjercicioInterfaces.Estrategias.ctdEstrategias
{
    public class ctPagoConTransferencia : IPagoCardTransferencia
    {
        public Tarjeta? tarjeta;
        public Cobrador cobrador;
        public CuentaDni ctdni;

        public ctPagoConTransferencia(Cobrador cobradorAsociado, CuentaDni ctaDni)
        {
            cobrador = cobradorAsociado;
            ctdni = ctaDni;
        }

        public bool PagoConTransferencia(decimal montoPagar, string cbu)
        {
            decimal descuentoTotal;
            decimal descuentoLyM = 0.15m;
            decimal descuentoS = 0.20m;

            if (cobrador == null || cobrador.cbu == null)
            {
                Console.WriteLine($"No existe el cbu que acaba de ingresar");
                return false;
            }

            string cbuCobrador = cobrador.retornarCbuCobrador();

            if (cbu.Length != 22)
            {
                Console.WriteLine("Error con el CBU!");
                return true;
            }

            if (cbuCobrador != cbu)
            {
                Console.WriteLine($"Error ese cbu no existe!");
                return false;
            }

            DateTime hoy = DateTime.Now;
            var region = new CultureInfo("es-ES");
            string diaSemana = hoy.ToString("dddd", region).ToLower();


            if (ctdni.saldoCuentaDni < montoPagar)
            {
                Console.WriteLine("Saldo insuficiente en Cuenta DNI.");
                return false;
            }

            if (diaSemana == "lunes" || diaSemana == "miércoles")
            {
                descuentoTotal = montoPagar - (descuentoLyM * montoPagar);
                ctdni.saldoCuentaDni -= descuentoTotal;
                cobrador.cobrarMonto(descuentoTotal);
                Console.WriteLine($"Resultado del monto a pagar con descuento aplicado: {descuentoTotal}");
                Console.WriteLine($"Le quedo un saldo de : {ctdni.saldoCuentaDni}");
                Console.WriteLine("Descuento del 15% aplicado por ser Lunes o Miércoles.");

                return true;
            }
            else if (diaSemana == "sábado")
            {
                descuentoTotal = montoPagar - (descuentoS * montoPagar);
                ctdni.saldoCuentaDni -= descuentoTotal;
                cobrador.cobrarMonto(descuentoTotal);
                Console.WriteLine($"Resultado del monto a pagar con descuento aplicado: {descuentoTotal}");
                Console.WriteLine($"Le quedo un saldo de : {ctdni.saldoCuentaDni}");
                Console.WriteLine("Descuento del 20% aplicado por ser sábado.");
                return true;
            }

            if (montoPagar > ctdni.limiteTransferencia)
            {
                Console.WriteLine($"El monto excede el límite de transferencia ${ctdni.limiteTransferencia}");
                return false;
            }

            if (montoPagar <= ctdni.saldoCuentaDni)
            {
                ctdni.saldoCuentaDni -= montoPagar;
                cobrador.cobrarMonto(montoPagar);
                Console.WriteLine("Exitoso el pago!");
                return true;
            }
            Console.WriteLine($"Error");
            return false;
        }

    }
}
