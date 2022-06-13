using FluentAssertions;
using Kata_Services.Commands.AddMessageWithMentionsToTimeline;
using Kata_Services.Commands.AddUser;
using Kata_Services.CommonViewModels;
using Kata_Services.Queries;
using MediatR;

namespace BechavioralTestsSpecFlow.Steps
{
    [Binding]
    public class MentionsStepDefinitions
    {
        private int _firstUserId;
        private int _secondUserId;
        private int _messageId;
        private AddUserViewModel _secondUser;
        private readonly IMediator _mediator;

        public MentionsStepDefinitions(IMediator mediator)
        {
            _mediator = mediator;
        }

        [Given(@"a first user name is ""([^""]*)""")]
        public void GivenAFirstUserNameIs(string bob)
        {
            var model = new AddUserViewModel
            {
                Name = bob,
                DisplayName = bob
            };

            _firstUserId = _mediator.Send(new AddUserCommand
            {
                NewUser = model
            }).Result;
        }

        [Given(@"a second user name is ""([^""]*)""")]
        public void GivenASecondUserNameIs(string charlie)
        {
            _secondUser = new AddUserViewModel
            {
                Name = charlie,
                DisplayName = charlie
            };

            _secondUserId = _mediator.Send(new AddUserCommand
            {
                NewUser = _secondUser
            }).Result;
        }

        [When(@"the first user mentions the second user in a message")]
        public void WhenTheFirstUserMentionsTheSecondUserInAMessage()
        {
            var model = new AddMessageWithMentionsViewModel
            {
                Content = "Bob mentions @Charlie. The message is parsed at the frontend",
                AuthorId = _firstUserId,
                Mentions = new List<UserInfoMentionsViewModel>
                {
                    new UserInfoMentionsViewModel
                    {
                        UserId = _secondUserId,
                        DisplayName = _secondUser.DisplayName,
                    }
                }
            };

            _messageId = _mediator.Send(new AddMessageWithMentionsCommand
            {
                NewPost = model
            }).Result;
        }

        [Then(@"the second user is added to mentions in message")]
        public void ThenTheSecondUserIsAddedToMentionsInMessage()
        {
            var timeline = _mediator.Send(new GetAllMessagesByUserQuery
            {
                UserId = _firstUserId
            }).Result;

            var message = timeline.FirstOrDefault(f => f.MessageId == _messageId);

            timeline.Should().NotBeNull();
            message.Should().NotBeNull();
            message.Mentions.Should().NotBeNull();
            message.Mentions.SingleOrDefault(f => f.DisplayName == _secondUser.DisplayName).Should().NotBeNull();
        }
    }
}
