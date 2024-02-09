using System.ComponentModel.DataAnnotations;

namespace EGG_Haunolding_Management_System.Models
{
    public class LoginViewModel
    {
        [Display(Name = "Benutzername")]
        [Required(ErrorMessage = "Gib einen Benutzernamen ein!")]
        public string? Username { get; set; }
        [Display(Name = "Password")]
        [Required(ErrorMessage = "Gib ein Passwort ein!")]
        public string? Password { get; set; }
    }

}