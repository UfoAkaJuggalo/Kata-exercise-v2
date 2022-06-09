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
        var message = CreateBaseMessage(content, author);
        
        AddMessageToUserTimeline(message, author);

        return message.MessageId;
    }

    public IEnumerable<Message> GetAllMessagesByUserId(int userId) =>
        _messages.Where(w => w.AuthorId == userId);

    public IEnumerable<Message> GetSortedByMessageIdFeedForUser(User user) =>
        _messages.Where(m => user.Subscriptions.Any(a => a.UserId == m.AuthorId)).OrderByDescending(o=>o.MessageId);

    public int AddMessageWithMentions(string content, User author, IEnumerable<User> mentions)
    {
        var message = CreateBaseMessage(content, author);
        message.Mentions = mentions;

        AddMessageToUserTimeline(message, author);

        return message.MessageId;
    }

    private Message CreateBaseMessage(string content, User author) =>
        new Message
        {
            MessageId = ++_idCounter,
            Content = content,
            Author = author,
            AuthorId = author.UserId,
            DateTime = DateTime.Now,
        };

    private void AddMessageToUserTimeline(Message message, User user)
    {
        user.Timeline.Add(message);
        _messages.Add(message);
    }
}