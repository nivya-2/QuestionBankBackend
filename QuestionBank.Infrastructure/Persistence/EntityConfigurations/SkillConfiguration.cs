using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using QuestionBank.Domain.Entities;

namespace QuestionBank.Infrastructure.Persistence.EntityConfigurations;

/// <summary>
/// Configures the Skill entity, including property requirements and seed data.
/// </summary>
public class SkillConfiguration : IEntityTypeConfiguration<Skill>
{
    public void Configure(EntityTypeBuilder<Skill> builder)
    {
        // Primary Key
        builder.HasKey(s => s.Id);
        builder.Property(s => s.Id).ValueGeneratedOnAdd();

        // Required properties
        builder.Property(s => s.Name).IsRequired();

        // Seed data
        builder.HasData(
            new Skill { Id = 1, Name = "C#"},
            new Skill { Id = 2, Name = "EF Core"},
            new Skill { Id = 3, Name = "Angular"}
        );
    }
}
