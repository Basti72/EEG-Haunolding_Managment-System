namespace EGG_Haunolding_Management_System.Class
{
    public interface IUserStore
    {
        UserItem? GetUser(string username, string password);

        bool AddUser(UserItem item);
    }
}