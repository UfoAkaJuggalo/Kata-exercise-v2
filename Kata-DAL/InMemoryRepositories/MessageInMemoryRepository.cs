using Kata_DAL.Entities;
using Kata_DAL.IRepositories;

namespace Kata_DAL.InMemoryRepositories;

public class MessageInMemoryRepository : IMessageRepository
{
    private readonly List<Message> _messages;
    private int _idCounter;

    public MessageInMemoryRepository()
    {
        _idCounter = 0;
        _messages = new List<Message>();
    }
    
    public int AddMessage(string content, User author)
    {
        var message = new Message
        {
            MessageId = ++_idCounter,
            Content = content,
            Author = author,
            AuthorId = author.UserId,
            DateTime = DateTime.Now
        };
        
        author.Timeline.Add(message);
        _messages.Add(message);

        return message.MessageId;
    }

    public IEnumerable<Message> GetAllMessagesByUserId(int userId) =>
        _messages.Where(w => w.AuthorId == userId);
}