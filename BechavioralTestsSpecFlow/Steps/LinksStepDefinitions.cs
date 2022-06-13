using FluentAssertions;
using Kata_Services.Commands.AddMessageV3ToTimeline;
using Kata_Services.Commands.AddUser;
using Kata_Services.CommonViewModels;
using Kata_Services.Queries;
using MediatR;

namespace BechavioralTestsSpecFlow.Steps
{
    [Binding]
    public class LinksStepDefinitions
    {
        private readonly IMediator _mediator;
        private int _userId;
        private AddMessageV3ToTimelineViewModel _message;
        private int _messageId;
        private string _linkAdress;
        private string _linkDescription;

        public LinksStepDefinitions(IMediator mediator)
        {
            _mediator = mediator;
        }

        [Given(@"user name is ""([^""]*)""")]
        public void GivenUserNameIs(string alice)
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


        [Given(@"message with text ""([^""]*)"" and link ""([^""]*)"" with description ""([^""]*)""")]
        public void GivenMessageWithTextAndLinkWithDescription(string p0, string p1, string github)
        {
            _linkAdress = p1;
            _linkDescription = github;

            _message = new AddMessageV3ToTimelineViewModel
            {
                Content = p0,
                AuthorId = _userId,
                Links = new List<LinksViewModel>
                {
                    new LinksViewModel
                    {
                        LinkId = 0,
                        Description = _linkDescription,
                        LinkAdress = _linkAdress
                    }
                }
            };
        }

        [When(@"User publish the message")]
        public void WhenUserPublishTheMessage()
        {
            _messageId = _mediator.Send(new AddMessageV3ToTimelineCommand
            {
                Model = _message
            }).Result;
        }

        [Then(@"message should be added to user timeline with links")]
        public void ThenMessageShouldBeAddedToUserTimelineWithLinks()
        {
            var timeline = _mediator.Send(new GetAllMessagesByUserQuery
            {
                UserId = _userId
            }).Result;

            var message = timeline.FirstOrDefault(f => f.MessageId == _messageId);

            timeline.Should().NotBeNullOrEmpty();
            message.Should().NotBeNull();
            message.Links.Should().NotBeNullOrEmpty();
            message.Links.SingleOrDefault(s => s.LinkAdress == _linkAdress && s.Description == _linkDescription).Should().NotBeNull();

        }
    }
}
