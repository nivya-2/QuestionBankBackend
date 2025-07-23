using FluentAssertions;
using MockQueryable.Moq;
using Moq;
using QuestionBank.Application.Contracts.Persistence;
using QuestionBank.Application.Dto;
using QuestionBank.Application.Features.QuestionManagement;
using QuestionBank.Domain.Entities;
using static QuestionBank.Shared.QuestionBankEnums;

namespace QuestionBankTest.Features.QuestionManagement;

/// <summary>
/// Unit test class for <see cref="GetQuestionsForInterviewQueryHandler"/>.
/// Validates the handler's ability to fetch the questions for a given interview
/// and map it to <see cref="QuestionUpdateDto">
/// </summary>
public class GetQuestionsForInterviewQueryHandlerTests
{
    /// <summary>
    /// Mocked instance of <see cref="IQuestionBankDbContext"/> used to simulate database operations.
    /// </summary>
    private readonly Mock<IQuestionBankDbContext> mockDbContext;

    /// <summary>
    /// The handler instance under test.
    /// </summary>
    private readonly GetQuestionsForInterviewQueryHandler handler;

    /// <summary>
    /// Initializes a new instance of the <see cref="GetQuestionsForInterviewQueryHandlerTests"/> class.
    /// Prepares mock data with interviews and questions for test validation.
    /// </summary>
    public GetQuestionsForInterviewQueryHandlerTests()
    {
        // Arrange
        mockDbContext = new Mock<IQuestionBankDbContext>();

        var interview = new Interview
        {
            Id = 1,
            Role = "Backend Developer",
            Status = InterviewStatus.Draft,
            Experience = 2.5m,
            CreatedBy = "Admin",
            CreatedOn = DateTime.UtcNow,
            InterviewSkills = new List<InterviewSkill>(),
            Questions = new List<InterviewQuestionDetail>
            {
                new InterviewQuestionDetail
                {
                    Id = 1,
                    InterviewId = 1,
                    Question = "What is Dependency Injection?",
                    CreatedOn = DateTime.UtcNow
                },
                new InterviewQuestionDetail
                {
                    Id = 2,
                    InterviewId = 1,
                    Question = "Explain SOLID principles.",
                    CreatedOn = DateTime.UtcNow
                }
            }
        };

        var interviews = new List<Interview> { interview };

        mockDbContext.Setup(c => c.Interviews)
            .Returns(interviews.AsQueryable().BuildMockDbSet().Object);

        handler = new GetQuestionsForInterviewQueryHandler(mockDbContext.Object);
    }
    /// <summary>
    /// Verify whether <see cref="GetQuestionsForInterviewQueryHandler"/> returns the correct
    /// list of see <see cref="QuestionUpdateDto"/> when the interview with the specified ID exists.
    /// </summary>
    [Fact]
    public async Task GetQuestionsForInterviewQueryHandler_ShouldReturnQuestions_WhenInterviewExists()
    {
        // Arrange
        var query = new GetQuestionsForInterviewQuery { InterviewId = 1 };

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(2);

        result.Should().ContainSingle(q => q.Id == 1 && q.Question == "What is Dependency Injection?");
        result.Should().ContainSingle(q => q.Id == 2 && q.Question == "Explain SOLID principles.");
    }

    /// <summary>
    /// Verify whether <see cref="GetQuestionsForInterviewQueryHandler"/> throws a <see cref="KeyNotFoundException"/>
    /// when the interview with the specified ID does not exist.
    /// </summary>
    [Fact]
    public async Task GetQuestionsForInterviewQueryHandler_ShouldThrowKeyNotFoundException_WhenInterviewNotFound()
    {
        // Arrange
        var query = new GetQuestionsForInterviewQuery { InterviewId = 99 };

        // Act & Assert
        var exception = await Assert.ThrowsAnyAsync<KeyNotFoundException>(
        async () => await handler.Handle(query, CancellationToken.None));

        // Additional Assert
        exception.Message.Should().Be("Interview with ID 99 not found.");
    }
}
