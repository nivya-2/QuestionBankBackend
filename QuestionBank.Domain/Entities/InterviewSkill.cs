using QuestionBank.Domain.Entities.Common;

namespace QuestionBank.Domain.Entities;

/// <summary>
/// Represents the association between an interview and a required skill.
/// This is a join entity in a many-to-many relationship between interviews and skills.
/// </summary>
public class InterviewSkill : BaseDomainEntity
{
    /// <summary>
    /// Gets or sets the foreign key referencing the associated interview.
    /// </summary>
    public int InterviewId { get; set; }

    /// <summary>
    /// Gets or sets the foreign key referencing the associated skill.
    /// </summary>
    public int SkillId { get; set; }

    /// <summary>
    /// Gets or sets the navigation property for the related interview.
    /// </summary>
    public Interview Interview { get; set; } = null!;

    /// <summary>
    /// Gets or sets the navigation property for the related skill.
    /// </summary>
    public Skill Skill { get; set; } = null!;
}
