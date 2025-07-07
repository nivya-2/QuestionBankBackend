using Microsoft.EntityFrameworkCore;
using QuestionBank.Application.Contracts.Persistence;
using QuestionBank.Domain.Entities;
using QuestionBank.Domain.Entities.Common;
using QuestionBank.Infrastructure.Persistence.EntityConfigurations;

namespace QuestionBank.Infrastructure;


/// <summary>
/// The database context for the QuestionBank application.
/// Manages entity sets and applies configurations.
/// </summary>
public class QuestionBankDbContext : DbContext, IQuestionBankDbContext
{
    public QuestionBankDbContext(DbContextOptions<QuestionBankDbContext> options)
        : base(options)
    {
    }

    /// <summary>
    /// Applies entity configurations during model creation.
    /// </summary>
    /// <param name="modelBuilder">The builder used to construct the model.</param>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Apply the configurations explicitly
        modelBuilder.ApplyConfiguration(new InterviewConfiguration());
        modelBuilder.ApplyConfiguration(new QuestionConfiguration());
        modelBuilder.ApplyConfiguration(new SkillConfiguration());
        modelBuilder.ApplyConfiguration(new InterviewSkillConfiguration());
    }
    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        foreach (var entry in ChangeTracker.Entries<BaseDomainEntity>())
        {
            entry.Entity.UpdatedOn = DateTime.UtcNow;

            if (entry.State == EntityState.Added)
            {
                entry.Entity.CreatedOn = DateTime.UtcNow;
            }
        }

        return base.SaveChangesAsync(cancellationToken);
    }

    // DbSets for each entity
    public DbSet<Interview> Interviews { get; set; }
    public DbSet<Skill> Skills { get; set; }
    public DbSet<InterviewSkill> InterviewSkills { get; set; }
    public DbSet<InterviewQuestionDetails> Questions { get; set; }

}
