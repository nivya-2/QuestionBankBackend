using QuestionBank.Domain.Entities.Common;

namespace QuestionBank.Domain.Entities;

/// <summary>
/// Represents a skill that can be associated with a interview role.
/// </summary>
public class Skill : BaseDomainEntity
{
    /// <summary>
    /// Name of the skill (e.g., "C#", "Angular").
    /// </summary>
    /// <example>C#</example>
    public string Name { get; set; } = null!;
}
