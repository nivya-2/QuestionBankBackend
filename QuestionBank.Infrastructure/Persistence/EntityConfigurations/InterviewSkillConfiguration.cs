using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using QuestionBank.Domain.Entities;

namespace QuestionBank.Infrastructure.Persistence.EntityConfigurations;

/// <summary>
/// Configures the InterviewSkill entity, including relationships and seed data.
/// </summary>
public class InterviewSkillConfiguration : IEntityTypeConfiguration<InterviewSkill>
{
    public void Configure(EntityTypeBuilder<InterviewSkill> builder)
    {
        // Primary Key
        builder.HasKey(i => i.Id);
        builder.Property(i => i.Id).ValueGeneratedOnAdd();

        // Relationships
        builder.HasOne(ik => ik.Interview)
            .WithMany(i => i.InterviewSkills)
            .HasForeignKey(ik => ik.InterviewId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(ik => ik.Skill)
            .WithMany()
            .HasForeignKey(ik => ik.SkillId)
            .OnDelete(DeleteBehavior.Restrict);

        // Seed data
        builder.HasData(
            new InterviewSkill { Id = 1, InterviewId = 1, SkillId = 1 },
            new InterviewSkill { Id = 2, InterviewId = 1, SkillId = 2 }
        );
    }
}
