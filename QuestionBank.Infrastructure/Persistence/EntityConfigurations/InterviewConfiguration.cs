using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using QuestionBank.Domain.Entities;
using static QuestionBank.Shared.QuestionBankEnums;

namespace QuestionBank.Infrastructure.Persistence.EntityConfigurations;

/// <summary>
/// Configures the Interview entity for EF Core, including property settings and seed data.
/// </summary>
public class InterviewConfiguration : IEntityTypeConfiguration<Interview>
{
    public void Configure(EntityTypeBuilder<Interview> builder)
    {
        // Primary key
        builder.HasKey(i => i.Id);
        builder.Property(i => i.Id).ValueGeneratedOnAdd();

        // Required properties
        builder.Property(i => i.Role).IsRequired();
        builder.Property(i => i.Status).IsRequired();

        builder.Property(i => i.Experience).HasColumnType("float");

        // Seed data
        builder.HasData( new Interview
        {
            Id = 1,
            Role = "Backend Developer",
            Status = InterviewStatus.Draft,
            Experience = 2,
            CreatedBy = "seeder",
            CreatedOn = new DateTime(2025, 06, 27, 0, 0, 0, DateTimeKind.Utc),
            UpdatedBy = "seeder",
            UpdatedOn = new DateTime(2025, 06, 27, 0, 0, 0, DateTimeKind.Utc)
        });
    }
}
