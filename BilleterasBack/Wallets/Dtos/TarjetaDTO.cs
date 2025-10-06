using BilleterasBack.Wallets.Shared;
using System.Text.Json.Serialization;

namespace BilleterasBack.Wallets.Dtos
{
    public class TarjetaDTO
    {
        [JsonPropertyName("numTarjeta")]
        public string NumeroTarjeta { get; set; }

        [JsonPropertyName("nombre")]
        public string Nombre { get; set; }

        [JsonPropertyName("apellido")]
        public string Apellido { get; set; }

        [JsonPropertyName("dni")]
        public int Dni { get; set; }

        [JsonPropertyName("fechaVenc")]
        public DateTime fechaVenc { get; set; }

        [JsonPropertyName("cod")]
        public int Cod { get; set; }

        public TipoMetodoPago TipoMetodoPago { get; set; }
    }
 }

