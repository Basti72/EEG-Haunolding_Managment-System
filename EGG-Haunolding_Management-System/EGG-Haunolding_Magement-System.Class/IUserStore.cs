using EGG_Haunolding_Management_System.Class;
using System.Runtime.CompilerServices;

public interface IUserStore
{
    UserItem GetUser(string username, string password);
    IEnumerable<UserItem> GetUsers();
    void AddUser(string username, string password);
    void ChangePassword(string username, string oldpw, string newpw, string confnewpw);
    void RemoveUser(string username);
}