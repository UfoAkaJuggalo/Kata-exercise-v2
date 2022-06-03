using Kata_DAL.Entities;
using Kata_DAL.InMemoryRepositories;
using Kata_DAL.IRepositories;
using NUnit.Framework;

namespace Unit_Tests_Kata_Services.RepositoryTest;

[TestFixture]
public class MessageInMemoryRepositoryTest
{
    private IMessageRepository _messageRepository;
    private User _user;
    
    [SetUp]
    public void Setup()
    {
        _messageRepository = new MessageInMemoryRepository();
        
        _user = new User
        {
            Name = "Alice",
            UserId = 0,
            DisplayName = "Alice",
            LastName = "Smith",
            Email = "a@s.com",
            Timeline = new List<Message>()
        };
    }
    
    [Test]
    public void NewMessageAdded()
    {
        // 1. Arrange
        // 2. Act

        var result = _messageRepository.AddMessage("some text", _user);
        // 3. Assert
        Assert.That(result, Is.GreaterThanOrEqualTo(0));
    }    
    [Test]
    public void NewMessageAddedToUserTimeline()
    {
        // 1. Arrange
        // 2. Act

        var messageId = _messageRepository.AddMessage("some text", _user);
        var timelineMessage = _user.Timeline.FirstOrDefault(x => x.MessageId == messageId);
        
        // 3. Assert
        Assert.That(timelineMessage, Is.Not.Null);
    }
}