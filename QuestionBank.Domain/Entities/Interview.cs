using QuestionBank.Domain.Entities.Common;
using static QuestionBank.Shared.QuestionBankEnums;

namespace QuestionBank.Domain.Entities;

public class Interview : BaseDomainEntity2
{
    public string Role { get; set; } = null!;
    public InterviewStatus Status { get; set; } = InterviewStatus.New!;
    public float? Experience { get; set; }

    public ICollection<InterviewSkill> InterviewSkills { get; set; } = new List<InterviewSkill>();
    public ICollection<Question> Questions { get; set; } = new List<Question>();
}
