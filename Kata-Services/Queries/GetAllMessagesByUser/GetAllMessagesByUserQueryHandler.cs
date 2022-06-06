using AutoMapper;
using Kata_DAL.Entities;
using Kata_DAL.IRepositories;
using MediatR;

namespace Kata_Services.Queries;

public class GetAllMessagesByUserQueryHandler : IRequestHandler<GetAllMessagesByUserQuery, IEnumerable<GetAllMessagesByUserViewModel>>
{
    private readonly IMessageRepository _messageRepository;
    private readonly IMapper? _mapper;

    public GetAllMessagesByUserQueryHandler(IMessageRepository messageRepository, IMapper? mapper)
    {
        _messageRepository = messageRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<GetAllMessagesByUserViewModel>> Handle(GetAllMessagesByUserQuery request, CancellationToken cancellationToken) =>
        _messageRepository.GetAllMessagesByUserId(request.UserId)
            .Select(message => _mapper.Map<GetAllMessagesByUserViewModel>(message)).ToList();
}