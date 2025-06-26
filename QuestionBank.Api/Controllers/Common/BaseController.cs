using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace QuestionBank.Api.Controllers.Common;

/// <summary>
/// Base Controller for all controllers.
/// </summary>
/// <seealso cref="ControllerBase" />
[Route("api")]
[ApiController]
[ProducesResponseType(typeof(ProblemDetails), 500)]
public class BaseController : ControllerBase
{
    private IMediator _mediator;
    protected IMediator Mediator => _mediator ??= HttpContext.RequestServices.GetService<IMediator>();
}
