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
    
    [Test]
    public void GetAllMessagesByUserIdIsEmpty()
    {
        // 1. Arrange
        _messageRepository.AddMessage("The first message", _user);
        _messageRepository.AddMessage("The seconf message", _user);
        
        // 2. Act

        var result = _messageRepository.GetAllMessagesByUserId(25); 
        
        // 3. Assert
        Assert.That(result, Is.Empty);
    }
    
    [Test]
    public void GetAllMessagesByUserIdIsNotNullOrEmpty()
    {
        // 1. Arrange
        _messageRepository.AddMessage("The first message", _user);
        _messageRepository.AddMessage("The seconf message", _user);
        
        // 2. Act

        var result = _messageRepository.GetAllMessagesByUserId(_user.UserId); 
        
        // 3. Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result, Is.Not.Empty);
    }
    
    [Test]
    public void GetAllMessagesByUserIdReturnsAllMessages()
    {
        // 1. Arrange
        _messageRepository.AddMessage("The first message", _user);
        _messageRepository.AddMessage("The seconf message", _user);
        
        // 2. Act

        var result = _messageRepository.GetAllMessagesByUserId(_user.UserId); 
        
        // 3. Assert
        Assert.That(result, Has.Exactly(2).Items);
    }
    
    [Test]
    public void GetAllMessagesByUserIdRerurnAddedMessages()
    {
        // 1. Arrange
        _messageRepository.AddMessage("The first message", _user);
        _messageRepository.AddMessage("The seconf message", _user);
        
        // 2. Act

        var result = _messageRepository.GetAllMessagesByUserId(_user.UserId); 
        
        // 3. Assert
        Assert.That(result, Has.Exactly(1).Matches<Message>(x => x.Content == "The first message"));
        Assert.That(result, Has.Exactly(1).Matches<Message>(x => x.Content == "The seconf message"));
    }
}