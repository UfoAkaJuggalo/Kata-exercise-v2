using MediatR;

namespace Kata_Services.Commands.AddMessageWithMentionsToTimeline;

public class AddMessageWithMentionsCommand : IRequest<int>
{
    public AddMessageWithMentionsViewModel NewPost { get; set; }
}
    
    
    