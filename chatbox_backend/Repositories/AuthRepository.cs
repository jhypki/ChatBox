using Models;
using Data;
using MongoDB.Driver;
namespace Repositories;
public class AuthRepository : IAuthRepository
{
    private readonly IMongoCollection<User> _users;
    public AuthRepository(MongoDbService mongoDbService)
    {
        _users = mongoDbService.Database.GetCollection<User>("Users");
    }

    public async Task<User> Register(User user)
    {
        await _users.InsertOneAsync(user);
        return user;
    }

    public async Task<User> Login(string email, string password)
    {
        var user = await _users.Find(u => u.Email == email).FirstOrDefaultAsync();
        if (user == null)
        {
            throw new Exception("User not found.");
        }
        if (user.Password != password)
        {
            throw new Exception("Incorrect password.");
        }
        return user;
    }

    public async Task<User> GetUserByEmail(string email)
    {
        return await _users.Find(u => u.Email == email).FirstOrDefaultAsync();
    }

    public async Task<User> GetUserById(string id)
    {
        return await _users.Find(u => u.Id == id).FirstOrDefaultAsync();
    }

    public async Task<User> UpdateUser(User user)
    {
        await _users.ReplaceOneAsync(u => u.Id == user.Id, user);
        return user;
    }

    public async Task<bool> DeleteUser(string id)
    {
        var result = await _users.DeleteOneAsync(u => u.Id == id);
        return result.DeletedCount > 0;
    }

    public async Task<bool> UserExists(string email)
    {
        var user = await _users.Find(u => u.Email == email).FirstOrDefaultAsync();
        return user != null;
    }
    public async Task<List<User>> GetUsersByIds(string[] userIds)
    {
        return await _users.Find(u => userIds.Contains(u.Id)).ToListAsync();
    }

}