namespace QuestionBank.Domain.Entities.Common;

public abstract class BaseDomainEntity
{
    public int Id { get; set; }

    public DateTime? CreatedOn { get; set; }

    public DateTime? UpdatedOn { get; set; }

   
}
