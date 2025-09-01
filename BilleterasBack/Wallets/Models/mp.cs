using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BilleterasBack.Wallets.Models
{
    public class Mp
    {
        [Key]
        public int id_mp { get; set; }

        [ForeignKey("Usuario")]
        public int? id_usuario { get; set; }
        public string nombre { get; set; }
        public string apellido { get; set; }
        public int dni { get; set; }
       
        public string? cvu_mp { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal saldo_cuenta_mercado_pago { get; set; }
        public DateTime fecha_creacion { get; set; } = DateTime.Now;
        public bool activo { get; set; } = true;
        public virtual Usuario? Usuario { get; set; }
    }
}
