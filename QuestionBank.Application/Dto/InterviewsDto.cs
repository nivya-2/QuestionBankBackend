namespace QuestionBank.Application.Dto;

/// <summary>
/// Represents the interview details shown to the client or user.
/// </summary>
public class InterviewsDto
{
    /// <summary>
    /// Specifies the unique identifier for the interview.
    /// <example>1</example>
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Specifies the role for which the interview is conducted.
    /// <example>Software Engineer</example>
    /// </summary>
    public string Role { get; set; } = null!;

    /// <summary>
    /// Specifies the date and time when the interview was created.
    /// <example>2025-06-30T14:45:00</example>
    /// </summary>
    public DateTime? CreatedOn { get; set; }

    /// <summary>
    /// Specifies the name of the user who created the interview.
    /// <example>seeder</example>
    /// </summary>
    public string? CreatedBy { get; set; }

    /// <summary>
    /// Specifies the current status of the interview.
    /// <example>Scheduled</example>
    /// </summary>
    public string Status { get; set; }
}
