using static QuestionBank.Shared.QuestionBankEnums;

namespace QuestionBank.Application.Dto;

/// <summary>
/// Represents a single question item to be added, updated, or deleted for an interview.
/// </summary>
public class QuestionUpdateDto
{
    /// <summary>
    /// Question ID. Set to 0 if it’s a new question.
    /// </summary>
    /// <example>0</example>
    public int Id { get; set; }

    /// <summary>
    /// Text of the question.
    /// </summary>
    /// <example>What is dependency injection in .NET?</example>
    public string QuestionText { get; set; } = null!;

    /// <summary>
    /// Type of change to be applied to the question.
    /// </summary>
    /// <remarks>
    /// - <c>Add</c>: Question will be created (Id must be 0).  
    /// - <c>Update</c>: Question with existing Id will be updated.  
    /// - <c>Delete</c>: Question with existing Id will be removed.  
    /// - <c>None</c>: No changes will be applied to this question.
    /// </remarks>
    public QuestionChangeType ChangeType { get; set; }
}
