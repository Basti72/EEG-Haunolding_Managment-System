namespace EGG_Haunolding_Management_System.Class
{
    public interface IUserStore
    {
        UserItem? GetUserWithPassword(string username, string password);

        bool AddUser(UserItem item);

        List<UserItem> GetUsers();

        void DeleteUser(string username);

        UserItem GetUser(string username);

        bool UpdateUser(string oldUsername, string newUsername, string role);

        void UpdatePassword(string username, string hash, string salt);
    }
}