using Microsoft.AspNetCore.Mvc;
using System.Net;
using QuestionBank.Application.Dto;
using QuestionBank.Application.Features.InterviewManagement;
using QuestionBank.Api.Controllers.Common;
using QuestionBank.Application.Dto;
using QuestionBank.Application.Features.InterviewManagement;
using System.Net;

namespace QuestionBank.Api.Controllers;

/// <summary>
/// Controller responsible for managing interview-related operations.
/// </summary>
public class QuestionBankController : BaseController
{
    /// <summary>
    /// Retrieves a list of all interviews.
    /// </summary>
    /// <returns>
    /// An <see cref="IActionResult"/> containing a list of <see cref="InterviewsDto"/> objects and an HTTP 200 OK status.
    /// </returns>
    [HttpGet("interviews")]
    [ProducesResponseType(typeof(List<InterviewsDto>), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> GetAllInterviews()
    {
        var result = await Mediator.Send(new GetAllInterviewsQuery());
        return Ok(result);
    }

    /// <summary>
    /// Gets the details of a specific interview by its ID.
    /// </summary>
    /// <param name="id">The ID of the interview to retrieve.</param>
    /// <returns>
    /// An <see cref="IActionResult"/> containing the interview details if found (HTTP 200 OK),
    /// or a not found message (HTTP 404 Not Found) if the interview does not exist.
    /// </returns>
    [HttpGet("interviews/{id:int}")]
    [ProducesResponseType(typeof(InterviewDto), (int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    public async Task<IActionResult> GetInterviewById([FromRoute] int id)
    {
        try
        {
            var result = await Mediator.Send(new GetInterviewByIdQuery { InterviewId = id });
            return Ok(result);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message); 
        }
    }

    /// <summary>
    /// Updates the details of an existing interview with the provided information.
    /// </summary>
    /// <param name="id">The ID of the interview to be updated.</param>
    /// <param name="command">An object containing the updated interview data such as role, status, experience, and associated skill IDs.</param>
    /// <returns>
    /// An <see cref="IActionResult"/> indicating the outcome:
    /// <list type="bullet">
    /// <item><description><see cref="StatusCodes.Status204NoContent"/> if the update is successful.</description></item>
    /// <item><description><see cref="StatusCodes.Status404NotFound"/> if the interview does not exist.</description></item>
    /// </list>
    /// </returns>
    [HttpPut("interviews/{id:int}")]
    [ProducesResponseType((int)HttpStatusCode.NoContent)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    public async Task<IActionResult> UpdateInterview([FromRoute] int id, [FromBody] UpdateInterviewCommand command)
    {
        command.InterviewId = id;
        await Mediator.Send(command);
        return NoContent();
    }
}
