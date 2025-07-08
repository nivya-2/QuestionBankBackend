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
    public string Question { get; set; }
}
