using MediatR;
using Microsoft.EntityFrameworkCore;
using QuestionBank.Application.Contracts.Persistence;
using QuestionBank.Application.Dto;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;


namespace QuestionBank.Application.Features.InterviewManagement;

#region LLD: UpdateInterviewCommand
/*
Overview:
---------
This command updates an existing interview record in the database, including:
- Basic details such as Role, Status, and Experience.
- Associated skill mappings through `InterviewSkills` by replacing existing ones with new skill IDs.

Process Flow:
-------------
1. The command is triggered with:
   - InterviewId: The ID of the interview to be updated.
   - Role: The new job title or position.
   - InterviewStatus: The updated status in string form (converted to enum).
   - Experience: Updated years of experience.
   - SkillIds: A list of skill IDs to associate with the interview.

2. The handler fetches the interview entity from the database using InterviewId,
   including existing InterviewSkill associations for cleanup.

3. If no interview is found:
   -> Return `false`, indicating the update failed.

4. If interview exists:
   - Update the `Role`, `Experience`, and `Status` (after parsing string to enum).
   - If enum parsing fails, retain the original status.

5. Remove all existing `InterviewSkills` linked to this interview to prevent duplication or orphaned records.

6. Loop through the provided SkillIds and create new `InterviewSkill` entries
   that associate the current interview with the updated skills.

7. Save all changes to the database in a single operation via `SaveChangesAsync`.

8. Return `true` to indicate a successful update.

Output:
-------
- Returns `true` if the interview is found and successfully updated (including skills).
- Returns `false` if no interview is found for the given InterviewId.

Design Considerations:
----------------------
- All updates and relationship changes are performed in a single transaction context.
- Uses eager loading (`Include`) for InterviewSkills to enable correct deletion and re-association.
- Handles enum parsing gracefully to avoid crashing on invalid string input.
- Clears and resets interview-skill links instead of trying to diff the lists, simplifying logic.
- Respects cancellation via `cancellationToken`.

*/
#endregion

/// <summary>
/// Command to update an existing interview.
/// </summary>
public class UpdateInterviewCommand : IRequest<bool>
{
    /// <summary>
    /// The ID of the interview to update.
    /// </summary>
    public int InterviewId { get; set; }

    /// <summary>
    /// Updated role/title for the interview.
    /// </summary>
    public string Role { get; set; } = null!;

    /// <summary>
    /// Updated status for the interview.
    /// </summary>
    public string InterviewStatus { get; set; } = null!;

    /// <summary>
    /// Updated experience value.
    /// </summary>
    public float Experience { get; set; }

    /// <summary>
    /// List of skill IDs to associate with the interview.
    /// </summary>
    public List<int> SkillIds { get; set; } = new();
}
/// <summary>
/// Handles the <see cref="UpdateInterviewCommand"/> to perform an update operation
/// on an existing interview entity, including its associated InterviewSkills.
/// </summary>
public class UpdateInterviewCommandHandler : IRequestHandler<UpdateInterviewCommand, bool>
{
    private readonly IQuestionBankDbContext _dbContext;

    public UpdateInterviewCommandHandler(IQuestionBankDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<bool> Handle(UpdateInterviewCommand request, CancellationToken cancellationToken)
    {
        // Retrieve the interview along with its existing InterviewSkills.
        // If the interview does not exist, return false indicating failure.
        var interview = await _dbContext.Interviews
            .Include(i => i.InterviewSkills)
            .FirstOrDefaultAsync(i => i.Id == request.InterviewId, cancellationToken);

        if (interview == null)
            return false;

        // Update basic properties (Role, Status, Experience).
        // Status is parsed from string to enum, with fallback to existing value if invalid.
        interview.Role = request.Role;
        interview.Status = Enum.TryParse(request.InterviewStatus, out Shared.QuestionBankEnums.InterviewStatus status)
            ? status
            : interview.Status;
        interview.Experience = request.Experience;

        // Replace existing InterviewSkills with a new set based on SkillIds.
        // Old associations are deleted explicitly to avoid orphaned records or FK constraint issues.
        var existingSkills = _dbContext.InterviewSkills
            .Where(s => s.InterviewId == interview.Id);
        _dbContext.InterviewSkills.RemoveRange(existingSkills);

        foreach (var skillId in request.SkillIds)
        {
            interview.InterviewSkills.Add(new Domain.Entities.InterviewSkill
            {
                InterviewId = interview.Id,
                SkillId = skillId
            });
        }

        // Persist all changes to the database in a single atomic operation.
        await _dbContext.SaveChangesAsync(cancellationToken);

        // Indicate successful update.
        return true;
    }
}
