namespace BilleterasBack.Wallets.Dtos
{
    public class UserDto
    {
        public string? Nombre { get; set; }
        public string? Apellido { get; set; }
        public string? Email { get; set; }
        public int? Dni { get; set; }

        public bool Success { get; set; }
        public string? Message { get; set; }

    }
}
