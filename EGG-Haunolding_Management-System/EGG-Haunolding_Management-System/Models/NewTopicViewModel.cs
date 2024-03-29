using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace EGG_Haunolding_Management_System.Models
{
    public class NewTopicViewModel
    {
        [Display(Name = "Topic")]
        [Required(ErrorMessage = "Du musst ein Topic eingeben!")]
        public string Topic { get; set; }
    }
}