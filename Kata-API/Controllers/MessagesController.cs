using Kata_Services.Commands.AddMessageToTimeline;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Kata_API.Controllers;

[ApiController]
[Route("[controller]")]
public class MessagesController : ControllerBase
{
    private readonly IMediator _mediator;

    public MessagesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    [Route("add")]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(int))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(string))]
    public async Task<IActionResult> Add(AddMessageViewModel model)
    {
        var result = await _mediator.Send(new AddMessageCommand
        {
            NewPost = model
        });

        return result > 0
            ? StatusCode(StatusCodes.Status201Created, result)
            : StatusCode(StatusCodes.Status500InternalServerError, "Unable to add a new post");
    }
}