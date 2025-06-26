using QuestionBank.Domain.Entities.Common;

namespace QuestionBank.Domain.Entities;

public class Question : BaseDomainEntity
{
    public int InterviewId { get; set; }
    public string QuestionText { get; set; } = null!;

    public Interview Interview { get; set; } = null!;
}
