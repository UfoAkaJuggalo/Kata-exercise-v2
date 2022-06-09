using AutoMapper;
using Kata_DAL.Entities;
using Kata_DAL.IRepositories;
using MediatR;

namespace Kata_Services.Commands.AddMessageWithMentionsToTimeline;

public class AddMessageWithMentionsCommandHandler : IRequestHandler<AddMessageWithMentionsCommand, int>
{
    private readonly IMessageRepository _messageRepository;
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;

    public AddMessageWithMentionsCommandHandler(IMessageRepository messageRepository, IUserRepository userRepository, IMapper mapper)
    {
        _messageRepository = messageRepository;
        _userRepository = userRepository;
        _mapper = mapper;
    }

    public Task<int> Handle(AddMessageWithMentionsCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var author = _userRepository.GetUserById(request.NewPost.AuthorId);
            return Task.FromResult(_messageRepository.AddMessageWithMentions(request.NewPost.Content, author,
                _mapper.Map<IEnumerable<User>>(request.NewPost.Mentions)));
        }
        catch (ArgumentOutOfRangeException e)
        {
            Console.WriteLine(e);
            return Task.FromResult(-1);
        }
    }
}