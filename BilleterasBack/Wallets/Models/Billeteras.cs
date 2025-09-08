using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace BilleterasBack.Wallets.Models
{
    [Table("Billetera")]
    public class Billetera
    {
        [Key]
        public int IdBilletera { get; set; }

        [ForeignKey("Usuario")]
        public int IdUsuario { get; set; }

        [JsonIgnore]
        public Usuario? Usuario { get; set; }

        [Required]
        [MaxLength(50)]
        public string? Tipo { get; set; } // MercadoPago, PayPal, CuentaDNI

        [Required, MaxLength(50)]
        public string? Cvu { get; set; }

        [Column(TypeName = "decimal(15,2)")]
        public decimal Saldo { get; set; } = 0;

        [NotMapped]
        public bool Success { get; set; }

        [NotMapped]
        public string Message { get; set; } = string.Empty;

        // Relación con Tarjetas
        public ICollection<Tarjeta> Tarjetas { get; set; } = new List<Tarjeta>();
    }
}

