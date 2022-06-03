using Kata_DAL.IRepositories;
using MediatR;

namespace Kata_Services.Commands.AddMessageToTimeline;

public class AddMessageCommandHandler : IRequestHandler<AddMessageCommand, int>
{
    private readonly IMessageRepository _messageRepository;
    private readonly IUserRepository _userRepository;

    public AddMessageCommandHandler(IMessageRepository messageRepository, IUserRepository userRepository)
    {
        _messageRepository = messageRepository;
        _userRepository = userRepository;
    }

    public Task<int> Handle(AddMessageCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var author = _userRepository.GetUserById(request.NewPost.AuthorId);
            return Task.FromResult(_messageRepository.AddMessage(request.NewPost.Content, author));
        }
        catch (ArgumentOutOfRangeException e)
        {
            Console.WriteLine(e);
            return Task.FromResult(-1);
        }
    }
}