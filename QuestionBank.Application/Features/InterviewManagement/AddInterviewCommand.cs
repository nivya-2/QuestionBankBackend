using MediatR;
using Microsoft.EntityFrameworkCore;
using QuestionBank.Application.Contracts.Persistence;
using QuestionBank.Domain.Entities;
using static QuestionBank.Shared.QuestionBankEnums;

namespace QuestionBank.Application.Features.InterviewManagement;

/// <summary>
/// Represents the information required to create a new interview.
/// </summary>
public class AddInterviewCommand : IRequest<int>
{
    /// <summary>
    /// Specifies the role for which the interview is being created.
    /// <example>Backend Developer</example>
    /// </summary>
    public string Role { get; set; }

    /// <summary>
    /// Specifies the status of the interview. Accepted values: Draft, Submitted.
    /// <example>Draft</example>
    /// </summary>
    public string InterviewStatus { get; set; }

    /// <summary>
    /// Specifies the years of experience required for the role.
    /// <example>2.5</example>
    /// </summary>
    public float Experience { get; set; }

    /// <summary>
    /// Specifies the list of skill IDs associated with the interview.
    /// <example>[1, 3, 5]</example>
    /// </summary>
    public List<int> SkillIds { get; set; } = new();

}

/// <summary>
/// Handles the <see cref="AddInterviewCommand"/> to create a new interview and assign associated skills.
/// </summary>
public class AddInterviewCommandHandler : IRequestHandler<AddInterviewCommand, int>
{
    private readonly IQuestionBankDbContext _questionBankDbContext;

    /// <summary>
    /// Initializes a new instance of the <see cref="AddInterviewCommandHandler"/> class.
    /// </summary>
    /// <param name="questionBankDbContext">The database context used to access interview and skill data.</param>
    public AddInterviewCommandHandler(IQuestionBankDbContext questionBankDbContext)
    {
        _questionBankDbContext = questionBankDbContext;
    }

    /// <summary>
    /// Processes the interview creation request by validating input, creating the interview, and saving it to the database.
    /// </summary>
    /// <param name="request">The command containing interview details.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>The ID of the newly created interview.</returns>
    /// <exception cref="ArgumentException">Thrown when the interview status is invalid or any skill ID is invalid.</exception>
    public async Task<int> Handle(AddInterviewCommand request, CancellationToken cancellationToken)
    {
        if (!Enum.TryParse(request.InterviewStatus, out InterviewStatus status))
        {
            throw new ArgumentException($"Invalid interview status: {request.InterviewStatus}");
        }

        var validSkillIds = await _questionBankDbContext.Skills
        .Where(s => request.SkillIds.Contains(s.Id))
        .Select(s => s.Id)
        .ToListAsync(cancellationToken);

        var invalidSkillIds = request.SkillIds.Except(validSkillIds).ToList();
        if (invalidSkillIds.Any())
        {
            throw new ArgumentException($"Invalid skill IDs: {string.Join(", ", invalidSkillIds)}");
        }

        var interview = new Interview
        {
            Role = request.Role,
            Status = status,
            Experience = request.Experience,
            InterviewSkills = validSkillIds.Select(skillId => new InterviewSkill
            {
                SkillId = skillId
            }).ToList()

        };

        _questionBankDbContext.Interviews.Add(interview);
            
        await _questionBankDbContext.SaveChangesAsync(cancellationToken);
        return interview.Id;
    }
}