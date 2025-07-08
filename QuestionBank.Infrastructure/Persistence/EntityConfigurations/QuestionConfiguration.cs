using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using QuestionBank.Domain.Entities;

namespace QuestionBank.Infrastructure.Persistence.EntityConfigurations;

/// <summary>
/// Configures the Question entity, including relationships and seed data.
/// </summary>
public class QuestionConfiguration : IEntityTypeConfiguration<Question>
{
    public void Configure(EntityTypeBuilder<Question> builder)
    {
        // Primary key
        builder.HasKey(q => q.Id);
        builder.Property(q => q.Id).ValueGeneratedOnAdd();

        // Required properties
        builder.Property(q => q.QuestionText).IsRequired();

        // Relationships 
        builder.HasOne(q => q.Interview)
            .WithMany(i => i.Questions)
            .HasForeignKey(q => q.InterviewId)
            .OnDelete(DeleteBehavior.Restrict);

        // Seed data 
        builder.HasData(
            new Question { Id = 1, InterviewId = 1, QuestionText = "Explain DI in C#" },
            new Question { Id = 2, InterviewId = 1, QuestionText = "What is EF Core?" }
        );
    }
}
