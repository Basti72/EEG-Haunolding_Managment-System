using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace EGG_Haunolding_Management_System.Models
{
    public class LoginViewModel
    {
        [Display(Name = "Benutzername")]
        [Required(ErrorMessage = "Must provide a valid Username")]
        public string? Username { get; set; }
        [Display(Name = "Password")]
        [Required(ErrorMessage = "Must provide a valid Password")]
        public string? Password { get; set; }
    }
}
