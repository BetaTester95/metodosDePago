using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BilleterasBack.Wallets.Models
{
    [Table("Tarjeta")]
    public class Tarjeta
    {
        [Key]
        public int IdTarjeta { get; set; }

        [ForeignKey("Billetera")]
        public int IdBilletera { get; set; }
        public Billetera Billetera { get; set; }

        [Required, MaxLength(20)]
        public string NumeroTarjeta { get; set; }

        [Required]
        public DateTime FechaVencimiento { get; set; }

        [Required, MaxLength(3)]
        public int CodigoSeguridad { get; set; }

        public decimal Saldo { get; set; } = 10000;

    }
}
