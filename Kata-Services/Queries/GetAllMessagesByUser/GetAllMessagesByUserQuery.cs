using MediatR;

namespace Kata_Services.Queries;

public class GetAllMessagesByUserQuery : IRequest<IEnumerable<GetAllMessagesByUserViewModel>>
{
    public int UserId { get; set; }
}