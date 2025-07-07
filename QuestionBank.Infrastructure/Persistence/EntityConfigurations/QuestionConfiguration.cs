using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using QuestionBank.Domain.Entities;

namespace QuestionBank.Infrastructure.Persistence.EntityConfigurations;

/// <summary>
/// Configures the Question entity, including relationships and seed data.
/// </summary>
public class QuestionConfiguration : IEntityTypeConfiguration<InterviewQuestionDetails>
{
    public void Configure(EntityTypeBuilder<InterviewQuestionDetails> builder)
    {
        // Primary key
        builder.HasKey(q => q.Id);
        builder.Property(q => q.Id).ValueGeneratedOnAdd();

        // Required properties
        builder.Property(q => q.Question).IsRequired();

        // Relationships 
        builder.HasOne(q => q.Interview)
            .WithMany(i => i.Questions)
            .HasForeignKey(q => q.InterviewId)
            .OnDelete(DeleteBehavior.Restrict);

        // Seed data 
        builder.HasData(
            new InterviewQuestionDetails { Id = 1, InterviewId = 1, Question = "Explain DI in C#" },
            new InterviewQuestionDetails { Id = 2, InterviewId = 1, Question = "What is EF Core?" }
        );
    }
}
