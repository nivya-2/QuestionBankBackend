using QuestionBank.Domain.Entities.Common;

namespace QuestionBank.Domain.Entities;

/// <summary>
/// Represents a skill that can be associated with an interview role.
/// </summary>
public class Skill : BaseDomainEntity
{
    /// <summary>
    /// Gets or sets the name of the skill (e.g., "C#", "Angular").
    /// </summary>
    /// <example>C#</example>
    public string Name { get; set; } = null!;
}
