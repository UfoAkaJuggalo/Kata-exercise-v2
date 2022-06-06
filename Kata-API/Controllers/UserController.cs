using Kata_Services.Commands.AddUser;
using Kata_Services.Commands.SubscribeTo;
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
    [ProducesErrorResponseType(typeof(string))]
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
    
    [HttpPost]
    [Route("subscribe")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesErrorResponseType(typeof(string))]
    public async Task<IActionResult> SubscribeTo(SubscribeToViewModel model)
    {
        if (model.FollowerId == model.TargetUserId)
            return StatusCode(StatusCodes.Status500InternalServerError, "You cannot subscribe to yourself");
        
        return await _mediator.Send(new SubscribeToCommand
        {
            Model = new SubscribeToViewModel
            {
                FollowerId = model.FollowerId,
                TargetUserId = model.TargetUserId
            }
        })
            ? Ok()
            : StatusCode(StatusCodes.Status500InternalServerError, "Unable to subscribe to a user");
    }
}