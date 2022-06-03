using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentAssertions;
using Kata_DAL.Entities;
using Kata_DAL.IRepositories;
using Kata_Services.Commands.AddMessageToTimeline;
using Kata_Services.Commands.AddUser;
using MediatR;
using TechTalk.SpecFlow;

namespace BechavioralTestsSpecFlow.Steps;

[Binding]
public sealed class PostingStepDefinitions
{
    // For additional details on SpecFlow step definitions see https://go.specflow.org/doc-stepdef

    private readonly IMediator _mediator;
    private readonly IUserRepository _userRepository;
    private int _userId;
    private int _messageId;
    private string _messageText;


    public PostingStepDefinitions(IMediator mediator, IUserRepository userRepository)
    {
        _mediator = mediator;
        _userRepository = userRepository;
    }

    [Given(@"User named ""(.*)""")]
    public async Task GivenUserNamed(string name)
    {
        var model = new AddUserViewModel
        {
            Name = name,
            DisplayName = name
        };
        
        _userId = await _mediator.Send(new AddUserCommand
        {
            NewUser = model
        });
    }

    [Given(@"the message with text ""(.*)""")]
    public void GivenTheMessageWithText(string message)
    {
        _messageText = message;
    }

    [When(@"Alice publish the message")]
    public async Task WhenAlicePublishTheMessage()
    {
        var model = new AddMessageViewModel
        {
            Content = _messageText,
            AuthorId = _userId
        };
        ;
        _messageId = await _mediator.Send(new AddMessageCommand
        {
            NewPost = model
        });
    }

    [Then(@"the message should be added to Alice timeline")]
    public void ThenTheMessageShouldBeAddedToAliceTimeline()
    {
        var user = _userRepository.GetUserById(_userId);
        var messageFromTimeline = user.Timeline.FirstOrDefault(x => x.MessageId == _messageId);

        messageFromTimeline.Should().NotBeNull();
        messageFromTimeline?.Content.Should().BeSameAs(_messageText);
    }
}