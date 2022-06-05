using AutoMapper;
using Kata_DAL.Entities;
using Kata_DAL.IRepositories;
using Kata_Services.Infrastructure;
using Kata_Services.Queries;
using Moq;
using NUnit.Framework;

namespace Unit_Tests_Kata_Services.QueriesTests;

[TestFixture]
public class GetAllMessagesByUserQueryHandlerTest
{
    private GetAllMessagesByUserQuery _query;
    private GetAllMessagesByUserQueryHandler _queryHandler;
    private IMapper _mapper;

    [SetUp]
    public void Setup()
    {
        var messageRepository = new Mock<IMessageRepository>();
        var returnList = new List<Message>
        {
            new Message
            {
                MessageId = 0,
                AuthorId = 0,
                Content = "test1",
                DateTime = DateTime.Now
            },
            new Message
            {
                MessageId = 1,
                AuthorId = 0,
                Content = "test2",
                DateTime = DateTime.Now
            }
        };

        if (_mapper == null)
        {
            var mapConfig = new MapperConfiguration(mc => mc.AddProfile(new AutoMapperProfile()));
            _mapper = mapConfig.CreateMapper();
        }
        
        messageRepository.Setup(x => x.GetAllMessagesByUserId(It.Is<int>(x => x == 0))).Returns(returnList);
        
        _query = new GetAllMessagesByUserQuery { UserId = 0 };
        _queryHandler = new GetAllMessagesByUserQueryHandler(messageRepository.Object, _mapper);
    }

    [Test]
    public void DoesNotThrowException()
    {
        // 1. Arrange
        var messageRepository = new Mock<IMessageRepository>();
        var queryHandler = new GetAllMessagesByUserQueryHandler(messageRepository.Object, _mapper);
        
        // 2. Act
        
        // 3. Assert
        Assert.DoesNotThrowAsync(() => queryHandler.Handle(_query, CancellationToken.None));
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
    public async Task ReturnedListHasOnlyTwoItems()
    {
        // 1. Arrange
        
        // 2. Act
        var result = await _queryHandler.Handle(_query, CancellationToken.None);
        
        // 3. Assert
        Assert.That(result, Has.Exactly(2).Items);
    }

    [Test]
    public async Task ReturnedListOffMessagesContainsOneMessage()
    {
        // 1. Arrange
        
        // 2. Act
        var result = await _queryHandler.Handle(_query, CancellationToken.None);
        
        // 3. Assert
        Assert.That(result, Has.Exactly(1).Matches<GetAllMessagesByUserViewModel>(x => x.Content == "test1"));
        Assert.That(result, Has.Exactly(1).Matches<GetAllMessagesByUserViewModel>(x => x.Content == "test2"));
    }   
}