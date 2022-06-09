using Kata_DAL.Entities;
using Kata_DAL.InMemoryRepositories;
using Kata_DAL.IRepositories;

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
        
    [Test]
    public void GetFeedDoesntThrowException()
    {
        // 1. Arrange
        _user.Subscriptions = new List<User>();
        
        // 2. Act

        // 3. Assert
        Assert.DoesNotThrow(()=>_messageRepository.GetSortedByMessageIdFeedForUser(_user));
    }
    
    [Test]
    public void GetFeedFromTwoUsersIsNotNullOrEmpty()
    {
        // 1. Arrange
        var messageIds = GenerateUsersAndMessages();
        
        // 2. Act
        var result = _messageRepository.GetSortedByMessageIdFeedForUser(_user);

        // 3. Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result, Is.Not.Empty);
    }
    
    [Test]
    public void GetFeedFromTwoUsersOnly()
    {
        // 1. Arrange
        var messageIds = GenerateUsersAndMessages();
        var author = new User
        {
            Name = "Ian",
            UserId = 3,
            DisplayName = "Ian",
            Timeline = new List<Message>(),
        };
        
        _messageRepository.AddMessage("Ian sample message", author);
        
        // 2. Act
        var result = _messageRepository.GetSortedByMessageIdFeedForUser(_user);

        // 3. Assert
        Assert.That(result, Has.Exactly(4).Items);
        Assert.That(result, Has.None.Matches<Message>(m => m.AuthorId == 3));
    }
    
    [Test]
    public void GetFeedFromTwoUsersIsSorted()
    {
        // 1. Arrange
        var messageIds = GenerateUsersAndMessages();
        
        // 2. Act
        var result = _messageRepository.GetSortedByMessageIdFeedForUser(_user);
        var firstPost = result.FirstOrDefault();
        var lastPost = result.LastOrDefault();

        // 3. Assert
        Assert.That(firstPost.MessageId, Is.EqualTo(messageIds.Last()));
        Assert.That(lastPost.MessageId, Is.EqualTo(messageIds.First()));
    }
    
    [Test]
    public void NewMessageWithMentionsAdded()
    {
        // 1. Arrange
        var author = new User
        {
            Name = "Bob",
            UserId = 1,
            DisplayName = "Bob",
            Timeline = new List<Message>(),
            Followers = new List<User>{_user}
        };
        var mentions = new List<User> { _user };
        
        // 2. Act
        var result = _messageRepository.AddMessageWithMentions("some text", author, mentions);
        
        // 3. Assert
        Assert.That(result, Is.GreaterThanOrEqualTo(0));
    }    
    
    [Test]
    public void CheckIfUserIsMentionedInMessagesInFeed()
    {
        // 1. Arrange
        var newMessages = GenerateUsersAndMessagesWithMentions();
        
        // 2. Act
        var result = _messageRepository.GetSortedByMessageIdFeedForUser(_user);
        
        // 3. Assert
        Assert.That(result,
            Has.Exactly(1).Matches<Message>(m =>
                m.Mentions != null && m.MessageId == newMessages[0] && m.Mentions.Any(a => a.UserId == _user.UserId)));
        Assert.That(result,
            Has.Exactly(1).Matches<Message>(m =>
                m.Mentions != null && m.MessageId == newMessages[1] && m.Mentions.Any(a => a.UserId == _user.UserId)));
        Assert.That(result,
            Has.Exactly(1).Matches<Message>(m =>
                m.Mentions != null && m.MessageId == newMessages[2] && m.Mentions.Any(a => a.UserId == _user.UserId)));
        Assert.That(result,
            Has.Exactly(1).Matches<Message>(m =>
                m.Mentions != null && m.MessageId == newMessages[3] && m.Mentions.Any(a => a.UserId == _user.UserId)));
        
    }    

    private int[] GenerateUsersAndMessages()
    {
        _user.Subscriptions = new List<User>();
        var author1 = new User
        {
            Name = "Bob",
            UserId = 1,
            DisplayName = "Bob",
            Timeline = new List<Message>(),
            Followers = new List<User>{_user}
        };
        var author2 = new User
        {
            Name = "Charlie",
            UserId = 2,
            DisplayName = "Charlie",
            Timeline = new List<Message>(),
            Followers = new List<User>{_user}
        };
        var messagesIds = new int [4];
        
        _user.Subscriptions.Add(author1);
        _user.Subscriptions.Add(author2);
        
        messagesIds[0] = _messageRepository.AddMessage("Bob message 1", author1);
        messagesIds[1] = _messageRepository.AddMessage("Bob message 2", author1);
        messagesIds[2] = _messageRepository.AddMessage("Charlie message 1", author2);
        messagesIds[3] = _messageRepository.AddMessage("Charlie message 2", author2);

        return messagesIds;
    }
    private int[] GenerateUsersAndMessagesWithMentions()
    {
        _user.Subscriptions = new List<User>();
        var mentions = new List<User> { _user };
        var author1 = new User
        {
            Name = "Bob",
            UserId = 1,
            DisplayName = "Bob",
            Timeline = new List<Message>(),
            Followers = new List<User>{_user}
        };
        var author2 = new User
        {
            Name = "Charlie",
            UserId = 2,
            DisplayName = "Charlie",
            Timeline = new List<Message>(),
            Followers = new List<User>{_user}
        };
        var messagesIds = new int [4];
        
        _user.Subscriptions.Add(author1);
        _user.Subscriptions.Add(author2);
        
        messagesIds[0] = _messageRepository.AddMessageWithMentions("Bob message 1", author1, mentions);
        messagesIds[1] = _messageRepository.AddMessageWithMentions("Bob message 2", author1, mentions);
        messagesIds[2] = _messageRepository.AddMessageWithMentions("Charlie message 1", author2, mentions);
        messagesIds[3] = _messageRepository.AddMessageWithMentions("Charlie message 2", author2, mentions);

        return messagesIds;
    }
}