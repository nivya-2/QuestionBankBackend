using MediatR;
using Microsoft.EntityFrameworkCore;
using QuestionBank.Application.Contracts.Persistence;
using QuestionBank.Domain.Entities;
using static QuestionBank.Shared.QuestionBankEnums;

namespace QuestionBank.Application.Features.InterviewManagement;

public class AddInterviewCommand : IRequest<int>
{
    public string Role { get; set; }
    public string InterviewStatus { get; set; }
    public float Experience { get; set; }
    public List<int> SkillIds { get; set; } = new();

}
public class AddInterviewCommandHandler : IRequestHandler<AddInterviewCommand, int>
{
    private readonly IQuestionBankDbContext _questionBankDbContext;

    public AddInterviewCommandHandler(IQuestionBankDbContext questionBankDbContext)
    {
        _questionBankDbContext = questionBankDbContext;
    }
    
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