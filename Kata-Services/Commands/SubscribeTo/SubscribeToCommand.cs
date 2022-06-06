using MediatR;

namespace Kata_Services.Commands.SubscribeTo;

public class SubscribeToCommand : IRequest<bool>
{
    public SubscribeToViewModel Model { get; set; }
}