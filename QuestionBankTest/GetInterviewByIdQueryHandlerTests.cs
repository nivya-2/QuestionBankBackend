using FluentAssertions;
using MockQueryable.Moq;
using Moq;
using QuestionBank.Application.Contracts.Persistence;
using QuestionBank.Application.Dto;
using QuestionBank.Application.Features.InterviewManagement;
using QuestionBank.Domain.Entities;
using static QuestionBank.Shared.QuestionBankEnums;

namespace QuestionBankTest;

/// <summary>
/// Unit test class for <see cref="GetInterviewByIdQueryHandler"/>.
/// Validates the handler's ability to fetch a specific interview by ID
/// and map it to <see cref="InterviewDto">
/// </summary>
public class GetInterviewByIdQueryHandlerTests
{
    /// <summary>
    /// Mocked instance of <see cref="IQuestionBankDbContext"/> used to simulate database operations.
    /// </summary>
    private readonly Mock<IQuestionBankDbContext> mockDbContext;

    /// <summary>
    /// Initializes a new instance of the <see cref="GetInterviewByIdQueryHandlerTests"/> class.
    /// Prepares mock data with interviews and skills for test validation.
    /// </summary>
    public GetInterviewByIdQueryHandlerTests()
    {
        // Arrange
        mockDbContext = new Mock<IQuestionBankDbContext>();

        var skill1 = new Skill { Id = 1, Name = "C#" };
        var skill2 = new Skill { Id = 2, Name = "Angular" };

        var interviews = new List<Interview>
        {
            new Interview
            {
                Id = 1,
                Role = "Backend Developer",
                Status = InterviewStatus.Draft,
                Experience = 2.5m,
                CreatedBy = "Admin",
                CreatedOn = DateTime.UtcNow,
                InterviewSkills = new List<InterviewSkill>
                {
                    new InterviewSkill { SkillId = 1, Skill = skill1 },
                    new InterviewSkill { SkillId = 2, Skill = skill2 }
                }
            }
        };

        mockDbContext.Setup(c => c.Interviews)
            .Returns(interviews.AsQueryable().BuildMockDbSet().Object);
    }

    /// <summary>
    /// Verifies that the <see cref="GetInterviewByIdQueryHandler"/> returns the correct
    /// <see cref="InterviewDto"/> when the interview with the specified ID exists.
    /// </summary>
    [Fact]
    public async Task GetInterviewByIdQueryHandler_WhenInterviewExists_ShouldReturnInterviewDto()
    {
        // Arrange
        var handler = new GetInterviewByIdQueryHandler(mockDbContext.Object);
        var query = new GetInterviewByIdQuery { InterviewId = 1 };

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Role.Should().Be("Backend Developer");
        result.Experience.Should().Be(2.5m);
        result.InterviewStatus.Should().Be("Draft");
        result.InterviewSkills.Should().BeEquivalentTo(new[] { "C#", "Angular" });
    }

    /// <summary>
    /// Verifies that the <see cref="GetInterviewByIdQueryHandler"/> throws
    /// a <see cref="KeyNotFoundException"/> when and interview with
    /// the provided interview ID does not exist.
    /// </summary>
    [Fact]
    public async Task GetInterviewByIdQueryHandler_WhenInterviewDoesNotExist_ShouldThrowKeyNotFoundException()
    {
        // Arrange
        var handler = new GetInterviewByIdQueryHandler(mockDbContext.Object);
        var query = new GetInterviewByIdQuery { InterviewId = 999 };

        // Act & Assert
        var exception = await Assert.ThrowsAnyAsync<KeyNotFoundException>(
            async () => await handler.Handle(query, CancellationToken.None));

        // Message Assert
        Assert.Equal("Interview with ID 999 not found.", exception.Message);
    }
}
