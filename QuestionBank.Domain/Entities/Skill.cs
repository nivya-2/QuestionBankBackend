using QuestionBank.Domain.Entities.Common;

namespace QuestionBank.Domain.Entities;

public class Skill : BaseDomainEntity
{
    public string Name { get; set; } = null!;
}
