using EGG_Haunolding_Management_System.Class;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace EGG_Haunolding_Management_System.Models
{
    public class EditViewModel
    {
        [Required(ErrorMessage = "Neuer Benutzer muss gesetzt werden")]
        public string Username {  get; set; }
        public string OriginalUsername {  get; set; }
        public string Role {  get; set; }
        public List<SelectListItem> Roles { get; } = new List<SelectListItem>
        {
            new SelectListItem { Value = "User", Text= "User"},
            new SelectListItem { Value = "Admin", Text= "Admin"},
        };
    }
}