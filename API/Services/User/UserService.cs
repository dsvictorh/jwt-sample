using API.Models;
using API.Utils;

namespace API.Services
{
    public class UserService : IUserService
    {
        private readonly Dictionary<string, string[]> users = new Dictionary<string, string[]>
        {
            { "victor", new string[]{ UserRoles.Admin, UserRoles.Developer} },
            { "adriana", new string[]{ UserRoles.Accountant } },
            { "carlos", new string[]{} }
        };

        public async Task<User> GetUser(string username)
        {
            if (this.users.TryGetValue(username, out string[] roles))
            {
                return new User
                {
                    Username = username,
                    Roles = roles
                };
            }

            return null;
        }
    }
}
