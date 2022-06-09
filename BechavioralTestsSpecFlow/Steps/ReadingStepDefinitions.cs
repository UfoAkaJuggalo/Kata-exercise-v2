using FluentAssertions;
using Kata_Services.Commands.AddMessageToTimeline;
using Kata_Services.Commands.AddUser;
using Kata_Services.Queries;
using MediatR;

namespace BechavioralTestsSpecFlow.Steps
{
    [Binding]
    public class ReadingStepDefinitions
    {
        private readonly IMediator _mediator;
        private int _writerUserId;
        private int _readerUserId;
        private IEnumerable<GetAllMessagesByUserViewModel> _timeline;
        private int[] _messageIds;

        public ReadingStepDefinitions(IMediator mediator)
        {
            _mediator = mediator;
        }

        [Given(@"The first user name is ""([^""]*)""")]
        public void GivenTheFirstUserNameIs(string alice)
        {
            var model = new AddUserViewModel
            {
                Name = alice,
                DisplayName = alice
            };

            _writerUserId = _mediator.Send(new AddUserCommand
            {
                NewUser = model
            }).Result;
        }

        [Given(@"The second user name is ""([^""]*)""")]
        public void GivenTheSecondUserNameIs(string bob)
        {
            var model = new AddUserViewModel
            {
                Name = bob,
                DisplayName = bob
            };

            _readerUserId =  _mediator.Send(new AddUserCommand
            {
                NewUser = model
            }).Result;
        }

        [Given(@"the first user write messages on her timeline")]
        public void GivenTheFirstUserWriteMessagesOnHerTimeline()
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

            _messageIds[0] = _mediator.Send(new AddMessageCommand
            {
                NewPost = message1
            }).Result;
            _messageIds[1] = _mediator.Send(new AddMessageCommand
            {
                NewPost = message2
            }).Result;
        }

        [When(@"the second user wants to see Alice timeline")]
        public void WhenTheSecondUserWantsToSeeAliceTimeline() =>
            _timeline = _mediator.Send(new GetAllMessagesByUserQuery
            {
                UserId = _writerUserId
            }).Result;

        [Then(@"he gets all messages from the first user timeline")]
        public void ThenHeGetsAllMessagesFromTheFirstUserTimeline()
        {
            _timeline.Should().NotBeNullOrEmpty();
            _timeline.Should().HaveCount(2);
            _timeline.SingleOrDefault(f => f.Content == "The first message").Should().NotBeNull();
            _timeline.SingleOrDefault(f => f.Content == "The second message").Should().NotBeNull();
        }

    }
}
