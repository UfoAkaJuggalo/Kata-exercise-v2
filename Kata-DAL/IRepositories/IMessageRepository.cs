using Kata_DAL.Entities;

namespace Kata_DAL.IRepositories;

public interface IMessageRepository
{
    int AddMessage(string content, User author);
}