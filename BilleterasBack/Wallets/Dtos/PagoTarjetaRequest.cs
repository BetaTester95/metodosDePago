using BilleterasBack.Wallets.Shared;

namespace BilleterasBack.Wallets.Dtos
{
    public class PagoTarjetaRequest
    {
        public TipoMetodoPago tipoMetodoPago { get; set; }
        public decimal montoPagar { get; set; }
        public int cantCuotas { get; set; }

    }
}
