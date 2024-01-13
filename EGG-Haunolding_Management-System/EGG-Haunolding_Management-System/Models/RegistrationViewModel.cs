using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace EGG_Haunolding_Management_System.Models
{
    public class RegistrationViewModel
    {
        [Display(Name = "Benutzername")]
        [Required(ErrorMessage = "Username is required!")]
        public string Username { get; set; }

        [Display(Name = "Passwort")]
        [Required(ErrorMessage = "Password is required!")]
        public string Password { get; set; }

        [BindProperty]
        [Display(Name = "Rolle: ")]
        public string Role { get; set; }

        public List<SelectListItem> Roles { get; } = new List<SelectListItem>
        {
            new SelectListItem { Value = "User", Text= "User"},
            new SelectListItem { Value = "Admin", Text= "Admin"},
        };
    }
}