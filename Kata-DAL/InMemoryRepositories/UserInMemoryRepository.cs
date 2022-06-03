using Kata_DAL.Entities;
using Kata_DAL.IRepositories;

namespace Kata_DAL.InMemoryRepositories;

public class UserInMemoryRepository : IUserRepository
{
    private readonly List<User> _users;
    private int _idCounter;

    public UserInMemoryRepository()
    {
        _users = new List<User>();
        _idCounter = 0;
    }
    
    public int AddUser(string displayName, string name, string lastName, string email)
    {
        var user = new User
        {
            UserId = ++_idCounter,
            Name = name,
            DisplayName = displayName,
            LastName = lastName,
            Email = email,
            Timeline = new List<Message>()
        };
        
        _users.Add(user);

        return user.UserId;
    }

    public User GetUserById(int userId) =>
        _users.FirstOrDefault(f => f.UserId == userId) ??
        throw new ArgumentOutOfRangeException($"UserId: {userId} is not exist in our system");
}