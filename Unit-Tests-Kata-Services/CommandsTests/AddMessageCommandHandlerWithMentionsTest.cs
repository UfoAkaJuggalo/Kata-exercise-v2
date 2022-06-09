using AutoMapper;
using Kata_DAL.IRepositories;
using Kata_Services.Commands.AddMessageWithMentionsToTimeline;
using Kata_Services.CommonViewModels;
using Kata_Services.Infrastructure;
using Moq;

namespace Unit_Tests_Kata_Services.CommandsTests;

[TestFixture]
public class AddMessageCommandHandlerWithMentionsTest
{
    private AddMessageWithMentionsCommandHandler _command;
    private AddMessageWithMentionsCommand _addMessageCommand;
    private UserInfoViewModel _user;
    private IMapper? _mapper;
    
    [SetUp]
    public void Setup()
    {
        var messageRepository = new Mock<IMessageRepository>();
        var userRepository = new Mock<IUserRepository>();
        
        _user = new UserInfoViewModel
        {
            Name = "Alice",
            UserId = 0,
            DisplayName = "Alice",
            LastName = "Smith",
        };
        
        if (_mapper == null)
        {
            var mapConfig = new MapperConfiguration(mc => mc.AddProfile(new AutoMapperProfile()));
            _mapper = mapConfig.CreateMapper();
        }

        
        _command = new AddMessageWithMentionsCommandHandler(messageRepository.Object, userRepository.Object, _mapper);

        _addMessageCommand = new AddMessageWithMentionsCommand
        {
            NewPost = new AddMessageWithMentionsViewModel
            {
                Content = "Some post",
                AuthorId = 0,
                Mentions = new List<UserInfoMentionsViewModel>
                {
                    new UserInfoMentionsViewModel
                    {
                        UserId = 2,
                        DisplayName = "Bob"
                    }
                }
            }
        };
    }
    
    [Test]
    public void DoesNotThrowException()
    {
        // 1. Arrange
        // 2. Act
        
        // 3. Assert
        Assert.DoesNotThrowAsync(async ()=>await _command.Handle(_addMessageCommand, CancellationToken.None));
    }
    
    [Test]
    public async Task NewMessageAdded()
    {
        // 1. Arrange
        
        // 2. Act
        var result = await _command.Handle(_addMessageCommand, CancellationToken.None);
        
        // 3. Assert
        Assert.That(result, Is.GreaterThanOrEqualTo(0));
    }
}