using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EGG_Haunolding_Management_System.Class
{
    class UserItem
    {
        string Hash { get; set; }
        string Salt { get; set; }
        string UserName { get; set; }
        string Role { get; set; }
    }
}
