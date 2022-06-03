using Kata_DAL.IRepositories;
using MediatR;

namespace Kata_Services.Commands.AddUser;

public class AddUserCommandHandler : IRequestHandler<AddUserCommand, int>
{
    private readonly IUserRepository _userRepository;

    public AddUserCommandHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public Task<int> Handle(AddUserCommand request, CancellationToken cancellationToken) =>
        Task.FromResult(_userRepository.AddUser(request.NewUser.DisplayName, request.NewUser.Name, request.NewUser.LastName,
            request.NewUser.Email));
}