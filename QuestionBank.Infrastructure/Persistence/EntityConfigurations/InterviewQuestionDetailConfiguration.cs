using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using QuestionBank.Domain.Entities;

namespace QuestionBank.Infrastructure.Persistence.EntityConfigurations;

/// <summary>
/// Configures the Question entity, including relationships and seed data.
/// </summary>
public class InterviewQuestionDetailConfiguration : IEntityTypeConfiguration<InterviewQuestionDetail>
{
    public void Configure(EntityTypeBuilder<InterviewQuestionDetail> builder)
    {
        //Setting the name explicitly after renaming the model
        builder.ToTable("InterviewQuestionDetails");

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
            new InterviewQuestionDetail { Id = 1, InterviewId = 1, Question = "Explain DI in C#" },
            new InterviewQuestionDetail { Id = 2, InterviewId = 1, Question = "What is EF Core?" }
        );
    }
}
