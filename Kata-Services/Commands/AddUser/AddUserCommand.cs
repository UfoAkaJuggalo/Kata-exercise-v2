using MediatR;

namespace Kata_Services.Commands.AddUser;

public class AddUserCommand : IRequest<int>
{
    public AddUserViewModel NewUser { get; set; }
}