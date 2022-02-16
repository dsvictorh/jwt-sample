using API.Models;

namespace API.Services
{
    public interface IUserService
    {
        Task<User> GetUser(string username);
    }
}
