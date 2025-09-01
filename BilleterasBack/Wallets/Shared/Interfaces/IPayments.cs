using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BilleterasBack.Wallets.Shared.Interfaces
{
    public interface IPagoCardTransferencia
    {
        bool PagoConTransferencia(decimal montoPagar, string cbu);
    }

    public interface IAgregarCard //AGREGAR TARJETA 
    {
        bool AgregarTarjeta(string numTarjeta, string nombre, string apellido, int dni, DateTime fechaVenc, int cod);
    }

    public interface IpagoCardCred //PARA PAGAR CON TARJETA DE CREDITO O DEBITO
    {
        bool PagoConTarjetaCredito(decimal montoPagar, int cantCuotas);

    }


}
