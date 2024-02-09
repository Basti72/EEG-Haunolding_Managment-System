using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace EGG_Haunolding_Management_System.Models
{
    public class RegistrationViewModel
    {
        [Display(Name = "Benutzername")]
        [Required(ErrorMessage = "Gib einen Benutzernamen ein!")]
        public string Username { get; set; }

        [Display(Name = "Passwort")]
        [Required(ErrorMessage = "Gib ein Passwort ein!")]
        public string Password { get; set; }

        [BindProperty]
        [Display(Name = "Rolle: ")]
        public string Role { get; set; }

        public List<SelectListItem> Roles { get; } = new List<SelectListItem>
        {
            new SelectListItem { Value = "User", Text= "User"},
            new SelectListItem { Value = "Admin", Text= "Admin"},
        };

        public List<int> SelectedTopicIds { get; set; } = new List<int>();

        public List<SelectListItem> AvailableTopics { get; set; } = new List<SelectListItem>();

        [Display(Name = "Zugriff auf alle Topics")]
        public bool AccessAllTopics { get; set; }
    }
}