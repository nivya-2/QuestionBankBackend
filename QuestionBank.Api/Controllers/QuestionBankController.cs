using Microsoft.AspNetCore.Mvc;
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
    /// Gets all interview details.
    /// </summary>
    /// <returns>A list of interviews with associated information.</returns>
    [HttpGet("interviews/{id:int}")]
    [ProducesResponseType(typeof(InterviewDto), (int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    public async Task<IActionResult> GetInterviewById([FromRoute] int id)
    {
        var result = await Mediator.Send(new GetInterviewByIdQuery { InterviewId = id });

        if (result == null)
            return NotFound();

        return Ok(result);
    }

    /// <summary>
    /// Updates an existing interview with new details.
    /// </summary>
    /// <param name="id">The ID of the interview to update.</param>
    /// <param name="command">The updated interview data.</param>
    /// <returns>No content if update is successful; NotFound otherwise.</returns>
    [HttpPut("interviews/{id:int}")]
    [ProducesResponseType((int)HttpStatusCode.NoContent)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    public async Task<IActionResult> UpdateInterview([FromRoute] int id, [FromBody] UpdateInterviewCommand command)
    {
        command.InterviewId = id;
        var success = await Mediator.Send(command);
        if (!success)
            return NotFound();

        return NoContent();
    }


}
