using FluentAssertions;
using MockQueryable.Moq;
using Moq;
using QuestionBank.Application.Contracts.Persistence;
using QuestionBank.Application.Features.InterviewManagement;
using QuestionBank.Domain.Entities;
using static QuestionBank.Shared.QuestionBankEnums;

namespace QuestionBankTest;

/// <summary>
/// Unit test class for <see cref="GetAllInterviewsQueryHandler"/>.
/// Verifies the behavior of the handler when interviews exist or do not exist in the data source.
/// </summary>
public class GetAllInterviewsQueryHandlerTests
{
    /// <summary>
    /// Mocked instance of <see cref="IQuestionBankDbContext"/> used to simulate database operations.
    /// </summary>
    private readonly Mock<IQuestionBankDbContext> mockDbContext;

    /// <summary>
    /// Initializes a new instance of the <see cref="GetAllInterviewsQueryHandlerTests"/> class.
    /// Seeds the mock database context with two interview records by default.
    /// </summary>
    public GetAllInterviewsQueryHandlerTests()
    {
        // Arrange
        mockDbContext = new Mock<IQuestionBankDbContext>();
        var interviews = new List<Interview>
        {
            new Interview { Id = 1, Role = "Backend Developer", Status = InterviewStatus.Draft, CreatedBy = "Admin", CreatedOn = DateTime.UtcNow },
            new Interview { Id = 2, Role = "Frontend Developer", Status = InterviewStatus.Draft, CreatedBy = "Admin", CreatedOn = DateTime.UtcNow }
        };

        mockDbContext.Setup(c => c.Interviews).Returns(interviews.AsQueryable().BuildMockDbSet().Object);
    }

    /// <summary>
    /// Verifies that the <see cref="GetAllInterviewsQueryHandler"/> returns all interviews
    /// when interviews exist in the mocked DbContext.
    /// </summary>
    /// <remarks>
    /// The test sets up a mocked DbContext with two interview records and verifies that:
    /// - The result is not null
    /// - The result contains exactly two records
    /// - The role and status fields match the seeded data
    /// </remarks>
    [Fact]
    public async Task GetAllInterviewsQueryHandler_WhenInterviewsExist_ShouldReturnListOfInterviews()
    {
        // Act
        var handler = new GetAllInterviewsQueryHandler(mockDbContext.Object);
        var result = await handler.Handle(new GetAllInterviewsQuery(), CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(2);
        result.First().Should().NotBeNull();
        result.First().Role.Should().Be("Backend Developer");
        result.First().Status.Should().Be("Draft");
        result.Last().Should().NotBeNull();
        result.Last().Role.Should().Be("Frontend Developer");
        result.Last().Status.Should().Be("Draft");
    }

    /// <summary>
    /// Verifies that the <see cref="GetAllInterviewsQueryHandler"/> returns an empty list
    /// when no interviews exist in the mocked DbContext.
    /// </summary>
    /// <remarks>
    /// This test ensures the handler gracefully handles an empty data source and
    /// returns an empty list instead of null or throwing an exception.
    /// </remarks>
    [Fact]
    public async Task GetAllInterviewsQueryHandler_WhenNoInterviewsExist_ShouldReturnEmptyList()
    {
        // Arrange
        mockDbContext.Setup(c => c.Interviews)
            .Returns(new List<Interview>().AsQueryable().BuildMockDbSet().Object);

        var handler = new GetAllInterviewsQueryHandler(mockDbContext.Object);

        // Act
        var result = await handler.Handle(new GetAllInterviewsQuery(), CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEmpty();
    }
}