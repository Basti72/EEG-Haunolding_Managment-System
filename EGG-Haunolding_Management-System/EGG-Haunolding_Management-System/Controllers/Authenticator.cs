using EGG_Haunolding_Management_System.Class;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using MySqlConnector;
using System.Net.Http;
using System.Reflection;
using System.Security.Claims;

namespace EGG_Haunolding_Management_System.Controllers;

public class Authenticator
{
    string Path = Directory.GetParent(Directory.GetCurrentDirectory()).FullName + "\\Resources\\ConnectionStringExtern.txt";

    IUserStore UserStore;
     Authenticator(IUserStore userStore)
    {
        UserStore = userStore;
    }
    public async Task<bool> Login(string username, string password, HttpContext httpContext)
    {
        UserItem? user = UserStore.GetUser(username, password);
        IUserStore store = new MySqUserStore(Path);
        if (user == null)
            return false;
        await httpContext.SignInAsync(
                new ClaimsPrincipal(
                    new ClaimsIdentity(
                        new[]
                        {
                            new Claim(ClaimTypes.Name, user.Username),
                            new Claim(ClaimTypes.Role, user.Role),
                        },
                        CookieAuthenticationDefaults.AuthenticationScheme
        )
                )
            );
        return true;
    }
    public string? TryGetLoggedInUser(HttpContext httpContext)
    {
        return httpContext.User.Identity?.Name;
    }
    public async Task Logout(HttpContext httpContext)
    {
        await httpContext.SignOutAsync();
    }
}
public class StaticUserStore : IUserStore
{
    public MySqlConnection Connection;
    public StaticUserStore()
    {
        Connection = new MySqlConnection("Server=127.0.0.1; Port=3307; Database=graduates;User=root;Password=1234");
    }
    public UserItem? GetUser(string username, string password)
    {
        var entry = new
        {
            Username = username,
            Password = password
        };
        return conn.QueryFirstOrDefault<UserItem>("SELECT * FROM User WHERE Username = @Username AND Password = @Password", entry);
    }

    public List<UserItem> GetUsers()
    {
        throw new NotImplementedException();
    }
}
