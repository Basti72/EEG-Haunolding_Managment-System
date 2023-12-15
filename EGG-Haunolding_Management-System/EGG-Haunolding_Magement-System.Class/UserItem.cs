using EGG_Haunolding_Management_System.Class;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EGG_Haunolding_Management_System.Class
{
    public class UserItem
    {
        public UserItem(string Username, string Password)
        {

        }
        public UserItem(string hash, string salt, string userName, string role, string password)
        {
            
        }
        public UserItem() { }
        public string Hash { get; set; }
        public string Salt { get; set; }
        public string Username { get; set; }
        public string Role { get; set; }
        public string Password { get; set; }
    }
}
