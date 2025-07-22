namespace QuestionBank.Application.Dto;

/// <summary>
/// Represents the skills shown to the user while adding an interview.
/// </summary>
public class SkillsDto
{
    /// <summary>
    /// Specifies the unique identifier for the skill.
    /// </summary>
    /// <example> 1 </example>
    public int Id { get; set; }

    /// <summary>
    /// Specifies the skill name.
    /// </summary>
    /// <example> C# </example>
    public string? Skill { get; set; }
}
