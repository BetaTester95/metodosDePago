using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace BilleterasBack.Wallets.Models
{
    [Table("Usuario")]
    public class Usuario
    {
        [Key]
        public int IdUsuario { get; set; }

        [Required(ErrorMessage ="El nombre es obligatorio"), MaxLength(100)]
        public string? Nombre { get; set; }

        [Required(ErrorMessage ="El apellido es obligatorio"), MaxLength(100)]
        public string? Apellido { get; set; }

        [Required(ErrorMessage ="El email es obligatorio"), MaxLength(100)]
        public string? Email { get; set; }

        [Required(ErrorMessage ="El password es obligatorio"), MaxLength(255)]
        public string? PasswordHash { get; set; }

        [Required(ErrorMessage ="El DNI es obligatorio")]
        public int? Dni { get; set; }

        [ForeignKey("TipoUsuario")]
        public int IdTipoUsuario { get; set; }
        
        public TipoUsuario? TipoUsuario { get; set; }

        public DateTime FechaRegistro { get; set; } = DateTime.Now;

        [Required]
        [MaxLength(20)]
        public string? Estado { get; set; } = "activo"; // activo, suspendido, eliminado

        // Relación con Billeteras
        [JsonIgnore]
        public ICollection<Billetera> Billeteras { get; set; } = new List<Billetera>();
    }
}
