using Kata_DAL.Entities;

namespace Kata_DAL.IRepositories;

public interface IUserRepository
{
    int AddUser(string displayName, string name, string lastName, string email);
    User GetUserById(int userId);
}