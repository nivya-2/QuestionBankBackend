namespace QuestionBank.Application.Dto;

/// <summary>
/// Data Transfer Object representing an interview with basic details and skills.
/// </summary>
public class InterviewDto
{
    /// <summary>
    /// Gets or sets the job role of the interview.
    /// </summary>
    public string Role { get; set; } = null!;

    /// <summary>
    /// Gets or sets the status of the interview as a string.
    /// </summary>
    public string InterviewStatus { get; set; } = null!;

    /// <summary>
    /// Gets or sets the experience required for the interview.
    /// </summary>
    public decimal? Experience { get; set; }

    /// <summary>
    /// Gets or sets the username or identifier of the user who created the interview.
    /// </summary>
    public string? CreatedBy { get; set; } = null!;

    /// <summary>
    /// Gets or sets the list of skill names associated with the interview.
    /// </summary>
    public List<string> InterviewSkills { get; set; } = new();
}
