using MediatR;

namespace Kata_Services.Commands.AddMessageToTimeline;

public class AddMessageCommand : IRequest<int>
{
    public AddMessageViewModel NewPost { get; set; }
}