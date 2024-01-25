using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;

namespace EGG_Haunolding_Management_System.Class
{
    public class AuthenticationLogic
    {
        private IUserStore UserStore { get; set; }
        public AuthenticationLogic(IUserStore userStore)
        {
            UserStore = userStore;
        }

        public async Task<bool> Login(string username, string password, HttpContext httpContext)
        {
            UserItem? user = UserStore.GetUserWithPassword(username, password);

            if (user == null)
                return false;

            await httpContext.SignInAsync(
                    new ClaimsPrincipal(
                        new ClaimsIdentity(
                            new[]
                            {
                                new Claim(ClaimTypes.Name, user.Username),
                                new Claim(ClaimTypes.Role, user.Role)
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

        public string? TryGetRoleOfUser(HttpContext httpContext)
        {
            if (httpContext.User.Identity.IsAuthenticated)
            {
                var roles = httpContext.User.FindAll(ClaimTypes.Role);
                if (roles != null && roles.Any())
                {
                    string userRole = roles.First().Value;
                    return userRole;
                }
                return "no role";
            }
            return "no role";
        }

        public async Task Logout(HttpContext httpContext)
        {
            await httpContext.SignOutAsync();
        }

    }
}