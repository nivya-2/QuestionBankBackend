namespace QuestionBank.Domain.Entities.Common;

/// <summary>
/// Extended base class that includes auditing information about the user who created or last updated the entity.
/// Inherits basic properties like Id, CreatedOn, and UpdatedOn from <see cref="BaseDomainEntity"/>.
/// </summary>
public class BaseDomainEntity2 : BaseDomainEntity
{
    /// <summary>
    /// Identifier (e.g., username or user ID) of the user who created the entity.
    /// </summary>
    /// <example>admin_user</example>
    public string? CreatedBy { get; set; }

    /// <summary>
    /// Identifier (e.g., username or user ID) of the user who last updated the entity.
    /// </summary>
    /// <example>reviewer_user</example>
    public string? UpdatedBy { get; set; }
}
