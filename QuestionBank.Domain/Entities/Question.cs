using QuestionBank.Domain.Entities.Common;

namespace QuestionBank.Domain.Entities;

/// <summary>
/// Represents a question that is part of an interview.
/// </summary>
public class Question : BaseDomainEntity
{
    /// <summary>
    /// Foreign key referencing the associated interview.
    /// </summary>
    public int InterviewId { get; set; }

    /// <summary>
    /// The text of the interview question.
    /// </summary>
    /// <example>What is EF Core?</example>
    public string QuestionText { get; set; } = null!;

    /// <summary>
    /// Navigation property for the related interview.
    /// </summary>
    public Interview Interview { get; set; } = null!;
}
