namespace BilleterasBack.Wallets.Dtos
{
    public class TarjetaDTO
    {
        public string? NumeroTarjeta { get; set; }
        public string? Nombre { get; set; }
        public string? Apellido { get; set; }
        public int Dni { get; set; }
        public DateTime FechaExp { get; set; }
        public int Cvv { get; set; }
    }
}
