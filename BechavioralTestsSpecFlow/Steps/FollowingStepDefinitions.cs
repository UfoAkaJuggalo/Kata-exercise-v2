using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Kata_Services.Commands.AddMessageToTimeline;
using Kata_Services.Commands.AddUser;
using Kata_Services.Commands.SubscribeTo;
using Kata_Services.Queries.GetFeed;
using MediatR;
using TechTalk.SpecFlow;

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

        [Given(@"the first user named ""(.*)""")]
        public async void GivenTheFirstUserNamedAlice(string name)
        {
            var model = new AddUserViewModel
            {
                Name = name,
                DisplayName = name
            };
        
            _firstUserId = await _mediator.Send(new AddUserCommand
            {
                NewUser = model
            });
        }

        [Given(@"Alice wrote message on her timeline")]
        public async void GivenAliceWroteMessageOnHerTimeline()
        {
            var message = new AddMessageViewModel
            {
                Content = "Alice wrote something",
                AuthorId = _firstUserId
            };
            
            _messageIds = new int[1];
            
            _messageIds[0] = await _mediator.Send(new AddMessageCommand
            {
                NewPost = message
            });
            
        }

        
        [Given(@"the second user named ""(.*)""")]
        public async void GivenTheSecondUserNamedCharlie(string name)
        {
            var model = new AddUserViewModel
            {
                Name = name,
                DisplayName = name
            };
        
            _secondUserId = await _mediator.Send(new AddUserCommand
            {
                NewUser = model
            });
        }

        [When(@"the second user subscribes to the first user")]
        public void WhenTheSecondUserSubscribesToTheFirstUser()
        {
            _mediator.Send(new SubscribeToCommand
            {
                Model = new SubscribeToViewModel
                {
                    FollowerId = _secondUserId,
                    TargetUserId = _firstUserId
                }
            });
        }

        [Then(@"he can get messages from the first user timeline")]
        public async Task ThenHeCanGetMessagesFromTheFirstUserTimeline()
        {
            var result = await _mediator.Send(new GetFeedQuery
            {
                UserId = _secondUserId
            });

            result.Should().HaveCount(1);
            result.Should().Satisfy(x => x.Author.UserId == _firstUserId);
        }

        [Given(@"the third user named ""(.*)""")]
        public async void GivenTheThirdUserNamedBob(string name)
        {
            var model = new AddUserViewModel
            {
                Name = name,
                DisplayName = name
            };
        
            _thirdUserId = await _mediator.Send(new AddUserCommand
            {
                NewUser = model
            });
        }

        [Given(@"the first user adds two messages")]
        public async void GivenTheFirstUserAddsTwoMessages()
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

        [Given(@"the third user adds two messages")]
        public async void GivenTheThirdUserAddsTwoMessages()
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
            _messageIds = new int[2];
            
            _messageIds[2] = await _mediator.Send(new AddMessageCommand
            {
                NewPost = message1
            });
            _messageIds[3] = await _mediator.Send(new AddMessageCommand
            {
                NewPost = message2
            });
        }

        [Given(@"the second user subscribes to the both users")]
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

        [When(@"the second users try to get his news feed")]
        public async Task WhenTheSecondUsersTryToGetHisNewsFeed()
        {
            _feed = await _mediator.Send(new GetFeedQuery
            {
                UserId = _secondUserId
            });
        }

        [Then(@"he get list of combined messages from his subscriptions")]
        public void ThenHeGetListOfCombinedMessagesFromHisSubscriptions()
        {
            _feed.Should().NotBeEmpty();
            _feed.Should().HaveCount(4);
            _feed.Should().HaveCount(2).And.Satisfy(x => x.Author.UserId == _firstUserId);
            _feed.Should().HaveCount(2).And.Satisfy(x => x.Author.UserId == _thirdUserId);
        }
    }
}
