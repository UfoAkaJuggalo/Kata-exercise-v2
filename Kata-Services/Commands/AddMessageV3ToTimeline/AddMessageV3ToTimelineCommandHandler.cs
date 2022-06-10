using AutoMapper;
using Kata_DAL.IRepositories;
using MediatR;

namespace Kata_Services.Commands.AddMessageV3ToTimeline;

public class AddMessageV3ToTimelineCommandHandler : IRequestHandler<AddMessageV3ToTimelineCommand, int>
{
    private readonly IMessageRepository _messageRepository;
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;

    public AddMessageV3ToTimelineCommandHandler(IMessageRepository messageRepository, IMapper mapper, IUserRepository userRepository)
    {
        _messageRepository = messageRepository;
        _mapper = mapper;
        _userRepository = userRepository;
    }

    public Task<int> Handle(AddMessageV3ToTimelineCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}