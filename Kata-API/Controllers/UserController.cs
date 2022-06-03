using Kata_Services.Commands.AddUser;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Kata_API.Controllers;

[ApiController]
[Route("[controller]")]
public class UserController : ControllerBase
{
    private readonly IMediator _mediator;

    public UserController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    [Route("add")]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(int))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(string))]
    public async Task<IActionResult> Add(AddUserViewModel model)
    {
        var result = await _mediator.Send(new AddUserCommand
        {
            NewUser = model
        });

        return result > 0
            ? StatusCode(StatusCodes.Status201Created, result)
            : StatusCode(StatusCodes.Status500InternalServerError, "Unable to add a new user");
    }
}