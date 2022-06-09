using FluentAssertions;
using Kata_Services.Commands.AddMessageToTimeline;
using Kata_Services.Commands.AddUser;
using Kata_Services.Commands.SubscribeTo;
using Kata_Services.Queries.GetFeed;
using MediatR;

namespace BechavioralTestsSpecFlow.Steps
{
    [Binding]
    public class FollowingStepDefinitions
    {
        private readonly IMediator _mediator;
        private int _firstUserId;
        private int _secondUserId;
        private int _thirdUserId;
        private int[] _messageIds;
        private IEnumerable<GetFeedViewModel> _feed;

        public FollowingStepDefinitions(IMediator mediator)
        {
            _mediator = mediator;
            _messageIds = new int[4];
        }

        [Given(@"the first user name is ""([^""]*)""")]
        public void GivenTheFirstUserNameIs(string alice)
        {
            var model = new AddUserViewModel
            {
                Name = alice,
                DisplayName = alice
            };

            _firstUserId = _mediator.Send(new AddUserCommand
            {
                NewUser = model
            }).Result;
        }

        [Given(@"the second user name is ""([^""]*)""")]
        public void GivenTheSecondUserNameIs(string charlie)
        {
            var model = new AddUserViewModel
            {
                Name = charlie,
                DisplayName = charlie
            };

            _secondUserId = _mediator.Send(new AddUserCommand
            {
                NewUser = model
            }).Result;
        }

        [Given(@"the first user writes messages on her timeline")]
        public void GivenTheFirstUserWritesMessagesOnHerTimeline()
        {
            var message = new AddMessageViewModel
            {
                Content = "Alice wrote something",
                AuthorId = _firstUserId
            };

            _messageIds = new int[1];

            _messageIds[0] = _mediator.Send(new AddMessageCommand
            {
                NewPost = message
            }).Result;
        }

        [When(@"The second user subscribes to the first user")]
        public void WhenTheSecondUserSubscribesToTheFirstUser() =>
            _mediator.Send(new SubscribeToCommand
            {
                Model = new SubscribeToViewModel
                {
                    FollowerId = _secondUserId,
                    TargetUserId = _firstUserId
                }
            });

        [Then(@"the second user can get messages from the first user timeline")]
        public void ThenTheSecondUserCanGetMessagesFromTheFirstUserTimeline()
        {
            var result = _mediator.Send(new GetFeedQuery
            {
                UserId = _secondUserId
            }).Result;

            result.Should().HaveCount(1);
            result.Should().Satisfy(x => x.Author.UserId == _firstUserId);
        }

        [Given(@"the third user name is ""([^""]*)""")]
        public void GivenTheThirdUserNameIs(string bob)
        {
            var model = new AddUserViewModel
            {
                Name = bob,
                DisplayName = bob
            };

            _thirdUserId = _mediator.Send(new AddUserCommand
            {
                NewUser = model
            }).Result;
        }

        [Given(@"The first user adds two messages")]
        public void GivenTheFirstUserAddsTwoMessages()
        {
            var message1 = new AddMessageViewModel
            {
                Content = "The first message by the first user",
                AuthorId = _firstUserId
            };
            var message2 = new AddMessageViewModel
            {
                Content = "The second message by the first user",
                AuthorId = _firstUserId
            };
            _messageIds = new int[4];

            _messageIds[0] = _mediator.Send(new AddMessageCommand
            {
                NewPost = message1
            }).Result;
            _messageIds[1] = _mediator.Send(new AddMessageCommand
            {
                NewPost = message2
            }).Result;
        }

        [Given(@"The third user adds two messages")]
        public void GivenTheThirdUserAddsTwoMessages()
        {
            var message1 = new AddMessageViewModel
            {
                Content = "The first message by the third user",
                AuthorId = _thirdUserId
            };
            var message2 = new AddMessageViewModel
            {
                Content = "The second message by the third user",
                AuthorId = _thirdUserId
            };

            _messageIds[2] = _mediator.Send(new AddMessageCommand
            {
                NewPost = message1
            }).Result;
            _messageIds[3] = _mediator.Send(new AddMessageCommand
            {
                NewPost = message2
            }).Result;
        }

        [Given(@"The second user subscribes to the both users")]
        public void GivenTheSecondUserSubscribesToTheBothUsers()
        {
            _mediator.Send(new SubscribeToCommand
            {
                Model = new SubscribeToViewModel
                {
                    FollowerId = _secondUserId,
                    TargetUserId = _firstUserId
                }
            });

            _mediator.Send(new SubscribeToCommand
            {
                Model = new SubscribeToViewModel
                {
                    FollowerId = _secondUserId,
                    TargetUserId = _thirdUserId
                }
            });
        }

        [When(@"the second user try to get his news feed")]
        public void WhenTheSecondUserTryToGetHisNewsFeed() =>
            _feed = _mediator.Send(new GetFeedQuery
            {
                UserId = _secondUserId
            }).Result;

        [Then(@"he get list of combined messages from both users")]
        public void ThenHeGetListOfCombinedMessagesFromBothUsers()
        {
            _feed.Should().NotBeEmpty();
            _feed.Should().HaveCount(4);
            _feed.Where(w => w.Author.UserId == _firstUserId).Should().HaveCount(2);
            _feed.Where(w => w.Author.UserId == _thirdUserId).Should().HaveCount(2);
        }
    }
}
