using QuestionBank.Domain.Entities.Common;

namespace QuestionBank.Domain.Entities;

/// <summary>
/// Represents a question that is part of an interview.
/// </summary>
public class Question : BaseDomainEntity
{
    /// <summary>
    /// Gets or sets the foreign key referencing the associated interview.
    /// </summary>
    public int InterviewId { get; set; }

    /// <summary>
    /// Gets or sets the text of the interview question.
    /// </summary>
    /// <example>What is EF Core?</example>
    public string QuestionText { get; set; } = null!;

    /// <summary>
    /// Gets or sets the navigation property for the related interview.
    /// </summary>
    public Interview Interview { get; set; } = null!;
}
