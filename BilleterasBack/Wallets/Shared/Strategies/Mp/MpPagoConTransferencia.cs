using EjercicioInterfaces.Pagos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EjercicioInterfaces.Estrategias;

public class MpPagoConTransferencia : IPagoCardTransferencia
{
    public Tarjeta? tarjeta;
    public Cobrador cobrador;
    public AppMp mp;

    public MpPagoConTransferencia(Cobrador cobradorAsociado, AppMp cuenta)
    {
        cobrador = cobradorAsociado;
        mp = cuenta;
    }


    public bool PagoConTransferencia(decimal montoPagar, string cbu)
    {

        if (cobrador == null || cobrador.cbu == null)
        {
            Console.WriteLine("No hay un cobrador asociado a ese CBU.");
            return false;
        }

        string cobradorCbu = cobrador.cbu;

        if (cbu.Length != 22)
        {
            return false;
        }

        if (cobradorCbu == cbu)
        {
            if (mp.saldoCuentaMercadoPago >= montoPagar)
            {
                mp.saldoCuentaMercadoPago -= montoPagar;
                cobrador.cobrarMonto(montoPagar);
                Console.WriteLine($"===== TRANSFERENCIA REALIZADA EXITOSAMENTE =====");
                Console.WriteLine($"\n");
                Console.WriteLine($"Se realizo una transferencia de: ${montoPagar} Pesos");
                Console.WriteLine($"Datos del Cobrador: {cobrador.cbu}.");
                Console.WriteLine($"Nombre Completo: {cobrador.nombre} {cobrador.apellido}");
                Console.WriteLine($"DNI: {cobrador.dni}");
                Console.WriteLine($"\n");
                Console.WriteLine($"===== TRANSFERENCIA REALIZADA EXITOSAMENTE =====");

                return true;
            }
            else
            {
                Console.WriteLine($"Saldo insuficiente. ${mp.saldoCuentaMercadoPago} Pesos.");
                return false;
            }
        }
        else
        {
            Console.WriteLine($"Error con el {cbu}");
            return false;
        }
    }
}
