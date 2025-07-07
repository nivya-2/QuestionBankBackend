using QuestionBank.Domain.Entities.Common;
using static QuestionBank.Shared.QuestionBankEnums;

namespace QuestionBank.Domain.Entities;

/// <summary>
/// Represents an interview, including details such as role, status, required experience,
/// and associated skills and questions.
/// </summary>
public class Interview : BaseDomainEntity2
{
    /// <summary>
    /// Gets or sets the job role or position for which the interview is being conducted.
    /// </summary>
    /// <example>Software Developer</example>
    public string Role { get; set; } = null!;

    /// <summary>
    /// Gets or sets the current status of the interview (e.g., New, Draft, Submitted).
    /// </summary>
    public InterviewStatus Status { get; set; } = InterviewStatus.Draft;

    /// <summary>
    /// Gets or sets the years of experience required or relevant for the interview.
    /// </summary>
    /// <example>3.5</example>
    public decimal? Experience { get; set; }

    /// <summary>
    /// Gets or sets the collection of skills associated with this interview.
    /// </summary>
    public List<InterviewSkill> InterviewSkills { get; set; } = new List<InterviewSkill>();

    /// <summary>
    /// Gets or sets the collection of questions linked to this interview.
    /// </summary>
    public ICollection<InterviewQuestionDetails> Questions { get; set; } = new List<InterviewQuestionDetails>();
}
