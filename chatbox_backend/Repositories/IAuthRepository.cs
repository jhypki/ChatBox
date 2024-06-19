using Models;
namespace Repositories;

public interface IAuthRepository
{
    Task<User> Register(User user);
    Task<User> Login(string email, string password);
    Task<User> GetUserByEmail(string email);
    Task<User> GetUserById(string id);
    Task<User> UpdateUser(User user);
    Task<bool> DeleteUser(string id);
    Task<bool> UserExists(string email);
    Task<List<User>> GetUsersByIds(string[] userIds);
}