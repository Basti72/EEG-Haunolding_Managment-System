namespace EGG_Haunolding_Management_System.Class
{
    public class UserItem
    {
        public string Username { get; set; }
        public string Hash { get; set; }
        public string Salt { get; set; }
        public string Role { get; set; }

        public UserItem(string username, string hash, string salt, string role)
        {
            Username = username;
            Hash = hash;
            Salt = salt;
            Role = role;
        }
    }
}