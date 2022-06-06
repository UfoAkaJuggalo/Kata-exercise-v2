using AutoMapper;
using Kata_DAL.Entities;
using Kata_DAL.IRepositories;
using Kata_Services.Infrastructure;
using Kata_Services.Queries.GetFeed;
using Moq;

namespace Unit_Tests_Kata_Services.QueriesTests;

[TestFixture]
public class GetFeedQueryHandlerTests
{
    private GetFeedQuery _query;
    private GetFeedQueryHandler _queryHandler;
    private IMapper? _mapper;

    [SetUp]
    public void Setup()
    {
        var messageRepository = new Mock<IMessageRepository>();
        var userRepository = new Mock<IUserRepository>();
        
        var user = new User
        {
            Name = "Alice",
            UserId = 0,
            DisplayName = "Alice",
            LastName = "Smith",
            Email = "a@s.com",
            Timeline = new List<Message>(),
            Subscriptions = new List<User>()
        };
        
        var author1 = new User
        {
            Name = "Bob",
            UserId = 1,
            DisplayName = "Bob",
            Timeline = new List<Message>(),
            Followers = new List<User>{user}
        };
        var author2 = new User
        {
            Name = "Charlie",
            UserId = 2,
            DisplayName = "Charlie",
            Timeline = new List<Message>(),
            Followers = new List<User>{user}
        };
        user.Subscriptions.Add(author1);
        user.Subscriptions.Add(author2);

        var feed = new List<Message>
        {
            new Message
            {
                MessageId = 0,
                AuthorId = 1,
                Content = "test1 one",
                DateTime = DateTime.Now,
                Author = author1
            },
            new Message
            {
                MessageId = 1,
                AuthorId = 2,
                Content = "test2 one",
                DateTime = DateTime.Now,
                Author = author2
            },
            new Message
            {
                MessageId = 2,
                AuthorId = 1,
                Content = "test1 two",
                DateTime = DateTime.Now,
                Author = author1
            },
            new Message
            {
                MessageId = 3,
                AuthorId = 2,
                Content = "test2 two",
                DateTime = DateTime.Now,
                Author = author2
            }
        };

        if (_mapper == null)
        {
            var mapConfig = new MapperConfiguration(mc => mc.AddProfile(new AutoMapperProfile()));
            _mapper = mapConfig.CreateMapper();
        }

        userRepository.Setup(x => x.GetUserById(It.Is<int>(x => x == 0))).Returns(user);
        messageRepository.Setup(x => x.GetSortedByMessageIdFeedForUser(It.Is<User>(x => x.UserId == 0)))
            .Returns(feed.OrderByDescending(o => o.MessageId));
        
        _query = new GetFeedQuery { UserId = 0 };
        _queryHandler = new GetFeedQueryHandler(messageRepository.Object, _mapper, userRepository.Object);
    }
    
    [Test]
    public void DoesNotThrowException()
    {
        // 1. Arrange
        // 2. Act
        // 3. Assert
        Assert.DoesNotThrowAsync(() => _queryHandler.Handle(_query, CancellationToken.None));
    }

    [Test]
    public async Task ReturnedListIsNotNullOrEmpty()
    {
        // 1. Arrange
        // 2. Act
        var result = await _queryHandler.Handle(_query, CancellationToken.None);
        
        // 3. Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result, Is.Not.Empty);
    }

    [Test]
    public async Task ReturnedListHasFourElements()
    {
        // 1. Arrange
        // 2. Act
        var result = await _queryHandler.Handle(_query, CancellationToken.None);
        
        // 3. Assert
        Assert.That(result, Has.Exactly(4).Items);
    }
}