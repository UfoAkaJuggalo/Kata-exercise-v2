using MediatR;

namespace Kata_Services.Queries.GetFeed;

public class GetFeedQuery : IRequest<IEnumerable<GetFeedViewModel>>
{
    public int UserId { get; set; }
}