using Models;
using System.Threading.Tasks;

namespace Services
{
    public interface IAuthService
    {
        Task<User> Register(User user);
        Task<User> Login(string email, string password);
        Task<User> GetUserByEmail(string email);
        Task<User> GetUserById(string id);
        Task<User> UpdateUser(User user);
        Task<bool> DeleteUser(string id);
        Task<bool> UserExists(string email);
        string GenerateJwtToken(User user);
        string HashPassword(string password);
        bool VerifyPassword(string password, string hash);
        Task<List<User>> GetUsersByIds(string[] userIds);
    }
}
