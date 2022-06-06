using Kata_Services.Commands.AddMessageToTimeline;
using Kata_Services.Queries;
using Kata_Services.Queries.GetFeed;
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
    [ProducesErrorResponseType(typeof(string))]
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

    [HttpGet]
    [Route("timeline/{id}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<GetAllMessagesByUserViewModel>))]
    public async Task<IEnumerable<GetAllMessagesByUserViewModel>> GetAllMessagesFromUserTimeline(int id)
    {
        return await _mediator.Send(new GetAllMessagesByUserQuery
        {
            UserId = id
        });
    }

    [HttpGet]
    [Route("feed")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<GetFeedViewModel>))]
    public async Task<IEnumerable<GetFeedViewModel>> GetUserFeed(int userId) =>
        await _mediator.Send(new GetFeedQuery { UserId = userId });
}