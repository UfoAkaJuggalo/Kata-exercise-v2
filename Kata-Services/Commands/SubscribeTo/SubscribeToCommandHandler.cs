using Kata_DAL.IRepositories;
using MediatR;

namespace Kata_Services.Commands.SubscribeTo;

public class SubscribeToCommandHandler : IRequestHandler<SubscribeToCommand, bool>
{
    private readonly IUserRepository _userRepository;

    public SubscribeToCommandHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<bool> Handle(SubscribeToCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var follower = _userRepository.GetUserById(request.Model.FollowerId);
            var isAlreadySubscribed = follower.Subscriptions.Any(a => a.UserId == request.Model.TargetUserId);

            if (!isAlreadySubscribed)
                _userRepository.SubscribeToUserTimeline(request.Model.FollowerId, request.Model.TargetUserId);
        }
        catch (ArgumentOutOfRangeException e)
        {
            Console.WriteLine(e);
            return false;
        }

        return true;
    }
}