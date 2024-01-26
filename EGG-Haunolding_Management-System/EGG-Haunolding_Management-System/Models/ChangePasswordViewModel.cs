using EGG_Haunolding_Management_System.Class;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.Net.Mail;

namespace EGG_Haunolding_Management_System.Models
{
    public class ChangePasswordViewModel
    {
        public string Username {  get; set; }

        [Required(ErrorMessage = "Sie müssen ein neues Passwort setzen!")]
        public string NewPassword { get; set; }

        [Required(ErrorMessage = "Bitte bestätigen Sie ihr Passwort!")]
        [Compare(nameof(NewPassword), ErrorMessage = "Passwörter stimmen nicht überein!")]
        public string ConfirmPassword { get; set;}
    }
}