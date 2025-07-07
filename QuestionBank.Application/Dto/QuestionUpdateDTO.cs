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

    /// <summary>
    /// Indicates whether this question should be deleted.
    /// </summary>
    /// <remarks>
    /// If <c>true</c>, the backend will delete the question with the provided <see cref="Id"/>.
    /// If <c>false:- </c>
    ///   -If Id == 0 → add new question.
    ///   -If Id > 0 → update existing question with new <see cref="QuestionText"/>.
    /// </remarks>
    public bool ShouldDelete { get; set; }
}
