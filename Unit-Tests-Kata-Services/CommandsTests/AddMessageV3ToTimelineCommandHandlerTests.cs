using AutoMapper;
using Kata_DAL.Entities;
using Kata_DAL.IRepositories;
using Kata_Services.Commands.AddMessageV3ToTimeline;
using Kata_Services.CommonViewModels;
using Kata_Services.Infrastructure;
using Moq;
using NUnit.Framework;

namespace Unit_Tests_Kata_Services.CommandsTests;

[TestFixture]
public class AddMessageV3ToTimelineCommandHandlerTests
{
    private IMapper? _mapper;
    private AddMessageV3ToTimelineCommand _command;
    private AddMessageV3ToTimelineCommand _commandWithMentions;
    private AddMessageV3ToTimelineCommandHandler _handler;

    [SetUp]
    public void Setip()
    {
        var messageRepository = new Mock<IMessageRepository>();
        var userRepository = new Mock<IUserRepository>();

        if (_mapper == null)
        {
            var mapConfig = new MapperConfiguration(mc => mc.AddProfile(new AutoMapperProfile()));
            _mapper = mapConfig.CreateMapper();
        }

        messageRepository.Setup(s => s.AddMessage(It.IsAny<string>(), It.IsAny<User>())).Returns(1);
        messageRepository.Setup(s => s.AddMessageWithMentions(It.IsAny<string>(), It.IsAny<User>(), It.IsAny<IEnumerable<User>>())).Returns(2);

        _handler = new AddMessageV3ToTimelineCommandHandler(messageRepository.Object, _mapper, userRepository.Object);
        
        _command = new AddMessageV3ToTimelineCommand
        {
            Model = new AddMessageV3ToTimelineViewModel
            {
                AuthorId = 1,
                Content = "Obczaj tego linka [link:1]",
                Links = new List<LinksViewModel>
                {
                    new LinksViewModel
                    {
                        Description = "Githubka",
                        LinkId = 1,
                        LinkAdress = @"https://github.com"
                    }
                }
            }
        };
        
        _commandWithMentions = new AddMessageV3ToTimelineCommand
        {
            Model = new AddMessageV3ToTimelineViewModel
            {
                AuthorId = 1,
                Content = "Obczaj tego linka [link:1]",
                Links = new List<LinksViewModel>
                {
                    new LinksViewModel
                    {
                        Description = "Githubka",
                        LinkId = 1,
                        LinkAdress = @"https://github.com"
                    }
                },
                Mentions = new List<UserInfoMentionsViewModel>
                {
                    new UserInfoMentionsViewModel
                    {
                        DisplayName = "Bob",
                        UserId = 2
                    }
                }
            }
        };
    }

    [Test]
    public void AddMessageWithoutMentionsDoesntThrowException()
    {
        // 1. Arrange
        // 2. Act

        // 3. Assert
        Assert.DoesNotThrow(async ()=> await _handler.Handle(_command, CancellationToken.None));
    }
    [Test]
    public async Task NewMessageWithoutMentionsAdded()
    {
        // 1. Arrange
        // 2. Act
        var result = await _handler.Handle(_command, CancellationToken.None);

        // 3. Assert
        Assert.That(result, Is.EqualTo(1));
    }
    [Test]
    public void AddMessageWithMentionsDoesntThrowException()
    {
        // 1. Arrange
        // 2. Act

        // 3. Assert
        Assert.DoesNotThrow(async () => await _handler.Handle(_commandWithMentions, CancellationToken.None));
    }
    [Test]
    public async Task NewMessageWithMentionsAdded()
    {
        // 1. Arrange
        // 2. Act
        var result = await _handler.Handle(_commandWithMentions, CancellationToken.None);

        // 3. Assert
        Assert.That(result, Is.EqualTo(2));
    }
}