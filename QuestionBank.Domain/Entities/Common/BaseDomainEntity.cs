namespace QuestionBank.Domain.Entities.Common;

/// <summary>
/// Base class for domain entities, providing common audit properties.
/// </summary>
public abstract class BaseDomainEntity
{
    /// <summary>
    /// Unique identifier for the entity.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Timestamp indicating when the entity was created.
    /// </summary>
    /// <example>2025-06-27T10:15:30Z</example>
    public DateTime? CreatedOn { get; set; }

    /// <summary>
    /// Timestamp indicating when the entity was last updated.
    /// </summary>
    /// <example>2025-06-27T12:45:10Z</example>
    public DateTime? UpdatedOn { get; set; }

    /// <summary>
    /// Indicates whether the entity has been soft deleted.
    /// </summary>
    /// <example>false</example>
    //public bool IsDeleted { get; set; } = false;


}
