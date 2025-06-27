using QuestionBank.Domain.Entities.Common;
using static QuestionBank.Shared.QuestionBankEnums;

namespace QuestionBank.Domain.Entities;

/// <summary>
/// Represents an interview, including details such as role, status, experience required,
/// and associated skills and questions.
/// </summary>
public class Interview : BaseDomainEntity2
{
    /// <summary>
    /// The job role or position for which the interview is being conducted.
    /// </summary>
    /// <example>Software Developer</example>
    public string Role { get; set; } = null!;

    /// <summary>
    /// The current status of the interview (e.g., New, Draft, Submitted).
    /// </summary>
    public InterviewStatus Status { get; set; } = InterviewStatus.New!;

    /// <summary>
    /// Years of experience required or relevant for the interview.
    /// </summary>
    /// <example>3.5</example>
    public float? Experience { get; set; }

    /// <summary>
    /// Collection of skills associated with this interview.
    /// </summary>
    public ICollection<InterviewSkill> InterviewSkills { get; set; } = new List<InterviewSkill>();

    /// <summary>
    /// Collection of questions linked to this interview.
    /// </summary>
    public ICollection<Question> Questions { get; set; } = new List<Question>();
}
