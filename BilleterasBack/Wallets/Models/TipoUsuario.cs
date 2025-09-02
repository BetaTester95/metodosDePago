using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BilleterasBack.Wallets.Models
{
    [Table("TipoUsuario")]
    public class TipoUsuario
    {
        [Key]
        public int IdTipoUsuario { get; set; }

        [Required]
        [MaxLength(50)]
        public string? NombreTipo { get; set; }

        // Relación con Usuario
        public ICollection<Usuario> Usuarios { get; set; } = new List<Usuario>();
    }
}
