using MediatR;

namespace Kata_Services.Commands.AddMessageV3ToTimeline;

public class AddMessageV3ToTimelineCommand : IRequest<int>
{
    public AddMessageV3ToTimelineViewModel Model { get; set; }
}