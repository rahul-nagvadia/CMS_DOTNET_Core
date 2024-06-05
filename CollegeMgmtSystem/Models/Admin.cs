using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace CollegeMgmtSystem.Models
{
    public class Admin
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Username is required")]
        [MaxLength(50)]
        public string Username { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [MaxLength(50)]
        public string Password { get; set; }

        public string UserId { get; set; }
        public IdentityUser User { get; set; }
    }
}
