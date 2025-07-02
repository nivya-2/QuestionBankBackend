using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Moq;
using QuestionBank.Application.Contracts.Persistence;
using QuestionBank.Application.Features.InterviewManagement;
using QuestionBank.Domain.Entities;
using QuestionBank.Infrastructure;
using static QuestionBank.Shared.QuestionBankEnums;


namespace QuestionBankTest;

/// <summary>
/// Unit test for <see cref="GetAllInterviewsQueryHandler"/> to verify it returns a list of interviews.
/// </summary>
public class GetAllInterviewsQueryHandlerTests
{
    /// <summary>
    /// Creates a fresh, in-memory DbContext and seeds it with data for testing.
    /// </summary>
    /// <returns>A fully configured and seeded IQuestionBankDbContext instance.</returns>
    private IQuestionBankDbContext CreateAndSeedDbContext()
    {
        // Configure the in-memory database options.
        var options = new DbContextOptionsBuilder<QuestionBankDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        // Create an instance of real DbContext.
        var dbContext = new QuestionBankDbContext(options);

        // Seed the database with test data.
        var data = new List<Interview>
        {
            new Interview { Id = 1, Role = "Backend Developer", Status = InterviewStatus.Draft, CreatedBy = "Admin", CreatedOn = DateTime.UtcNow },
            new Interview { Id = 2, Role = "Frontend Developer", Status = InterviewStatus.Draft, CreatedBy = "Admin", CreatedOn = DateTime.UtcNow }
        };

        dbContext.Interviews.AddRange(data);

        // Persist the data to the in-memory database
        dbContext.SaveChanges(); 

        return dbContext;
    }

    /// <summary>
    /// Verifies that the <see cref="GetAllInterviewsQueryHandler"/> returns a list of interviews
    /// when called with a valid, seeded in-memory database context.
    /// </summary>
    [Fact]
    public async Task GetAllInterviewsQueryHandler_WhenCalled_ShouldReturnListOfInterviews()
    {
        // ARRANGE
        var dbContext = CreateAndSeedDbContext();

        var handler = new GetAllInterviewsQueryHandler(dbContext);

        // ACT
        var result = await handler.Handle(new GetAllInterviewsQuery(), CancellationToken.None);

        // ASSERT
        result.Should().NotBeNull();
        result.Should().HaveCount(2);

        result.First().Should().NotBeNull();
        result.First().Role.Should().Be("Backend Developer");
        result.First().Status.Should().Be("Draft");

        result.Last().Should().NotBeNull();
        result.Last().Role.Should().Be("Frontend Developer");
        result.Last().Status.Should().Be("Draft");

    }
}