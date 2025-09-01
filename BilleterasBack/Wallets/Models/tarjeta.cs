using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace BilleterasBack.Wallets.Models
{
    [Table("tarjeta")]
    public class TarjetaEntity
    {
        [Key]
        public int id_tarjeta { get; set; }

        [ForeignKey("Usuario")]
        public int? id_usuario { get; set; }  // opcional, permite null si aún no se asigna

        public string numeroTarjeta { get; set; }

        [Column(TypeName = "varchar(50)")]
        public string nombreTitular { get; set; }

        [Column(TypeName = "varchar(50)")]
        public string apellidoTitular { get; set; }

        public int dniTitular { get; set; }

        public DateTime fechaVencimiento { get; set; }

        public int cod { get; set; }  // código de seguridad (CVV)

        [Column(TypeName = "varchar(20)")]
        public string? tipo_cuenta { get; set; }
        public DateTime fecha_creacion { get; set; } = DateTime.Now;

        public bool activo { get; set; } = true;

        public virtual Usuario? Usuario { get; set; }  // relación con usuario
    }
}
