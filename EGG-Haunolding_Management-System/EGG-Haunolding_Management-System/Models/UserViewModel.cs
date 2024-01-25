using EGG_Haunolding_Management_System.Class;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace EGG_Haunolding_Management_System.Models
{
    public class UserViewModel
    {
        public List<UserItem> Users { get; set; }
    }
}