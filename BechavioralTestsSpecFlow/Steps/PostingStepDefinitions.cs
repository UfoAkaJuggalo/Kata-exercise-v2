using FluentAssertions;
using Kata_DAL.IRepositories;
using Kata_Services.Commands.AddMessageToTimeline;
using Kata_Services.Commands.AddUser;
using MediatR;

namespace BechavioralTestsSpecFlow;

[Binding]
public sealed class PostingStepDefinitions
{
    // For additional details on SpecFlow step definitions see https://go.specflow.org/doc-stepdef

    private readonly IMediator _mediator;
    private readonly IUserRepository _userRepository;
    private int _userId;
    private int _messageId;
    private string _messageText;

    public PostingStepDefinitions(IUserRepository userRepository, IMediator mediator)
    {
        _userRepository = userRepository;
        _mediator = mediator;
    }

    [Given(@"the user name is ""([^""]*)""")]
    public void GivenTheUserNameIs(string alice)
    {
        var model = new AddUserViewModel
        {
            Name = alice,
            DisplayName = alice
        };

        _userId = _mediator.Send(new AddUserCommand
        {
            NewUser = model
        }).Result;
    }

    [Given(@"message with text ""([^""]*)""")]
    public void GivenMessageWithText(string p0)
    {
        _messageText = p0;
    }

    [When(@"user publish the message")]
    public void WhenUserPublishTheMessage()
    {
        var model = new AddMessageViewModel
        {
            Content = _messageText,
            AuthorId = _userId
        };
        ;
        _messageId = _mediator.Send(new AddMessageCommand
        {
            NewPost = model
        }).Result;
    }

    [Then(@"message should be added to Alice timeline")]
    public void ThenMessageShouldBeAddedToAliceTimeline()
    {
        var user = _userRepository.GetUserById(_userId);
        var messageFromTimeline = user.Timeline.FirstOrDefault(x => x.MessageId == _messageId);

        messageFromTimeline.Should().NotBeNull();
        messageFromTimeline?.Content.Should().BeSameAs(_messageText);
    }

}