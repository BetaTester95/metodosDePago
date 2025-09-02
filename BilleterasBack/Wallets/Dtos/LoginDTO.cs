using System.ComponentModel.DataAnnotations;

namespace BilleterasBack.Wallets.Dtos
{
    public class LoginDTO
    {
        [Required]
        public string Email { get; set; }

        [Required]
        public string PasswordHash { get; set; }

    }
}
