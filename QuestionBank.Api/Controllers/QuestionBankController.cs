using QuestionBank.Api.Controllers.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using QuestionBank.Application.Dto;
using QuestionBank.Application.Features.InterviewManagement;

namespace QuestionBank.Api.Controllers;

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
}
