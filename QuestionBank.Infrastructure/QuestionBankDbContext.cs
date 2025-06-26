using Microsoft.EntityFrameworkCore;
using QuestionBank.Application.Contracts.Persistence;
using QuestionBank.Domain.Entities.Common;

namespace QuestionBank.Infrastructure;

public class QuestionBankDbContext : DbContext, IQuestionBankDbContext
{
    public QuestionBankDbContext(DbContextOptions<QuestionBankDbContext> options)
        : base(options)
    {
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(QuestionBankDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        foreach (var entry in ChangeTracker.Entries<BaseDomainEntity>())
        {
            entry.Entity.UpdatedOn = DateTime.UtcNow;

            if (entry.State != EntityState.Added)
            {
                entry.Entity.CreatedOn = DateTime.UtcNow;
            }
        }

        return base.SaveChangesAsync(cancellationToken);
    }
}
