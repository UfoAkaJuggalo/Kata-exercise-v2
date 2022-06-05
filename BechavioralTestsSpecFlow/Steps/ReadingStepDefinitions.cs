using System;
using FluentAssertions;
using Kata_DAL.Entities;
using Kata_DAL.IRepositories;
using Kata_Services.Commands.AddMessageToTimeline;
using Kata_Services.Commands.AddUser;
using MediatR;
using TechTalk.SpecFlow;

namespace BechavioralTestsSpecFlow.Steps
{
    [Binding]
    public class ReadingStepDefinitions
    {
        private readonly IMediator _mediator;
        private readonly IMessageRepository _messageRepository;
        private int _writerUserId;
        private int _readerUserId;
        private IEnumerable<Message> _timeline;
        private int[] _messageIds;

        public ReadingStepDefinitions(IMessageRepository messageRepository, IMediator mediator)
        {
            _messageRepository = messageRepository;
            _mediator = mediator;
        }

        [Given(@"the writer user name is ""(.*)""")]
        public async void GivenTheWriterUserNameIs(string name)
        {
            var model = new AddUserViewModel
            {
                Name = name,
                DisplayName = name
            };
        
            _writerUserId = await _mediator.Send(new AddUserCommand
            {
                NewUser = model
            });
        }

        [Given(@"the reader user name is ""(.*)""")]
        public async void GivenTheReaderUserNameIs(string name)
        {
            var model = new AddUserViewModel
            {
                Name = name,
                DisplayName = name
            };
        
            _readerUserId = await _mediator.Send(new AddUserCommand
            {
                NewUser = model
            });
        }

        [Given(@"Alice write messages on her timeline")]
        public async void GivenAliceWriteMessagesOnHerTimeline()
        {
            var message1 = new AddMessageViewModel
            {
                Content = "The first message",
                AuthorId = _writerUserId
            };
            var message2 = new AddMessageViewModel
            {
                Content = "The second message",
                AuthorId = _writerUserId
            };
            _messageIds = new int[2];
            
            _messageIds[0] = await _mediator.Send(new AddMessageCommand
            {
                NewPost = message1
            });
            _messageIds[1] = await _mediator.Send(new AddMessageCommand
            {
                NewPost = message2
            });
        }

        [When(@"Bob wants to see Alice timeline")]
        public void WhenBobWantsToSeeAliceTimeline()
        {
            _timeline = _messageRepository.GetAllMessagesByUserId(_writerUserId);
        }

        [Then(@"he gets all messages from Alice timeline")]
        public void ThenHeGetsAllMessagesFromAliceTimeline()
        {
            _timeline.Should().NotBeNullOrEmpty();
            _timeline.Should().HaveCount(2);
            _timeline.Should().Satisfy(x => x.AuthorId == _writerUserId && x.Content == "The first message");
            _timeline.Should().Satisfy(x => x.AuthorId == _writerUserId && x.Content == "The second message");
            
        }
    }
}
