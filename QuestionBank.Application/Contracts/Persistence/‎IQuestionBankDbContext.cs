using Microsoft.EntityFrameworkCore;
using QuestionBank.Domain.Entities;

namespace QuestionBank.Application.Contracts.Persistence;

public interface IQuestionBankDbContext
{
    DbSet<Interview> Interviews { get; set; }
    DbSet<Skill> Skills { get; set; }
    DbSet<InterviewSkill> InterviewSkills { get; set; }
    DbSet<Question> Questions { get; set; }

}
