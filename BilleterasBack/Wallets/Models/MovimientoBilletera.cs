using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BilleterasBack.Wallets.Models
{
    public class MovimientoBilletera
    {
        [Key]
        public int IdMovimiento { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal Monto { get; set; }

        [Required]
        public DateTime FechaMovimiento { get; set; } = DateTime.Now;

        [Required, MaxLength(100)]
        public string? Descripcion { get; set; }

        // 🔗 Relación con Billetera
        [ForeignKey("Billetera")]
        public int IdBilletera { get; set; }
        public Billetera? Billetera { get; set; }

        // 🔗 Relación con TipoMovimiento
        [ForeignKey("TipoMovimiento")]
        public int IdTipoMovimiento { get; set; }
        public TipoMovimiento? TipoMovimiento { get; set; }

        // 🔗 (Opcional) Relación con Tarjeta si aplica al movimiento
        [ForeignKey("Tarjeta")]
        public int? IdTarjeta { get; set; }
        public Tarjeta? Tarjeta { get; set; }

    }
}
