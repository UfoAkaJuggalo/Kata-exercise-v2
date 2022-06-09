using AutoMapper;
using Kata_DAL.Entities;
using Kata_DAL.IRepositories;
using Kata_Services.CommonViewModels;
using MediatR;

namespace Kata_Services.Queries.GetFeed;

public class GetFeedQueryHandler : IRequestHandler<GetFeedQuery, IEnumerable<GetFeedViewModel>>
{
    private readonly IMessageRepository _messageRepository;
    private readonly IUserRepository _userRepository;
    private readonly IMapper? _mapper;

    public GetFeedQueryHandler(IMessageRepository messageRepository, IMapper? mapper, IUserRepository userRepository)
    {
        _messageRepository = messageRepository;
        _mapper = mapper;
        _userRepository = userRepository;
    }

    public async Task<IEnumerable<GetFeedViewModel>> Handle(GetFeedQuery request, CancellationToken cancellationToken)
    {
        var result = new List<GetFeedViewModel>();
        User user = new User();

        try
        {
            user = _userRepository.GetUserById(request.UserId);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
        
        var feed = _messageRepository.GetSortedByMessageIdFeedForUser(user);

        foreach (var item in feed)
        {
            var message = _mapper.Map<GetFeedViewModel>(item);
            var author = _mapper.Map<UserInfoViewModel>(item.Author);
            var mentions = _mapper.Map<IEnumerable<UserInfoViewModel>>(item.Mentions);

            message.Author = author;
            message.Mentions = mentions;

            result.Add(message);
        }

        return result;
    }
}