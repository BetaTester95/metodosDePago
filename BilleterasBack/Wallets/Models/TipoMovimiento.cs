using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace BilleterasBack.Wallets.Models
{
    public class TipoMovimiento
    {
        [Key]
        public int IdTipoMovimiento { get; set; }   
        [Required]
        public string? Descripcion { get; set; }

        public ICollection<MovimientoBilletera> Movimientos { get; set; } = new List<MovimientoBilletera>();

    }
}
