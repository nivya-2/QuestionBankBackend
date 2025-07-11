using FluentAssertions;
using MockQueryable.Moq;
using Moq;
using QuestionBank.Application.Contracts.Persistence;
using QuestionBank.Application.Dto;
using QuestionBank.Application.Features.QuestionManagement;
using QuestionBank.Domain.Entities;

namespace QuestionBankTest;

/// <summary>
/// Unit test class for <see cref="SetQuestionsCommandHandler"/>.
/// Validates handling of question insertions, updates, and deletions for interviews.
/// </summary>
public class SetQuestionsCommandHandlerTests
{
    /// <summary>
    /// Mocked instance of <see cref="IQuestionBankDbContext"/> used to simulate database operations.
    /// </summary>
    private readonly Mock<IQuestionBankDbContext> mockDbContext;

    /// <summary>
    /// Initializes a new instance of <see cref="SetQuestionsCommandHandlerTests"/> and sets up mock data.
    /// </summary>
    public SetQuestionsCommandHandlerTests()
    {
        //Arrange
        mockDbContext = new Mock<IQuestionBankDbContext>();

        var existingQuestions = new List<InterviewQuestionDetail>
        {
            new InterviewQuestionDetail { Id = 1, InterviewId = 1, Question = "Old Question 1" },
            new InterviewQuestionDetail { Id = 2, InterviewId = 1, Question = "Old Question 2" },
        };

        var interviews = new List<Interview>
        {
            new Interview
            {
                Id = 1,
                Questions = existingQuestions
            }
        };

        mockDbContext.Setup(db => db.Interviews)
            .Returns(interviews.AsQueryable().BuildMockDbSet().Object);

        mockDbContext.Setup(db => db.InterviewQuestionDetails)
            .Returns(existingQuestions.AsQueryable().BuildMockDbSet().Object);

        mockDbContext.Setup(db => db.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);
    }

    /// <summary>
    /// Verifies that <see cref="SetQuestionsCommandHandler"/> correctly handles a mixed set of operations:
    /// adding new questions, updating existing questions, and deleting questions.
    /// </summary>
    [Fact]
    public async Task SetQuestionsCommandHandler_WhenMixedOperations_ShouldUpdateCorrectly()
    {
        // Arrange
        var handler = new SetQuestionsCommandHandler(mockDbContext.Object);
        var command = new SetQuestionsCommand
        {
            InterviewId = 1,
            Questions = new List<QuestionUpdateDto>
            {
                new() { Id = 0, Question = "New Question 1" },
                new() { Id = 2, Question = "Updated Question 2" },
                new() { Id = 1, Question = "" }
            }
        };

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().BeTrue();

        mockDbContext.Verify(db => db.InterviewQuestionDetails.AddRange(It.IsAny<IEnumerable<InterviewQuestionDetail>>()), Times.Once);
        mockDbContext.Verify(db => db.InterviewQuestionDetails.RemoveRange(It.IsAny<IEnumerable<InterviewQuestionDetail>>()), Times.Once);
        mockDbContext.Verify(db => db.InterviewQuestionDetails.Update(It.IsAny<InterviewQuestionDetail>()), Times.Once);
        mockDbContext.Verify(db => db.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    /// <summary>
    /// Verifies that <see cref="SetQuestionsCommandHandler"/> throws <see cref="KeyNotFoundException"/>
    /// when the specified interview ID does not exist in the database.
    /// </summary>
    /// <remarks>
    /// Ensures that a 404-style failure is correctly triggered for invalid interview references.
    /// </remarks>
    [Fact]
    public async Task SetQuestionsCommandHandler_WhenInterviewNotFound_ShouldThrowKeyNotFoundException()
    {
        // Arrange
        var handler = new SetQuestionsCommandHandler(mockDbContext.Object);
        var command = new SetQuestionsCommand
        {
            InterviewId = 99,
            Questions = new List<QuestionUpdateDto> { new() { Id = 0, Question = "Test?" } }
        };

        // Act
        var exception = await Assert.ThrowsAsync<KeyNotFoundException>(() =>
            handler.Handle(command, CancellationToken.None));

        // Assert
        Assert.Equal("Interview with ID 99 not found.", exception.Message);
    }

    /// <summary>
    /// Verifies that <see cref="SetQuestionsCommandHandler"/> throws ArgumentException for empty new question.
    /// </summary>
    [Fact]
    public async Task SetQuestionsCommandHandler_WhenNewQuestionIsBlank_ShouldThrowArgumentException()
    {
        // Arrange
        var handler = new SetQuestionsCommandHandler(mockDbContext.Object);
        var command = new SetQuestionsCommand
        {
            InterviewId = 1,
            Questions = new List<QuestionUpdateDto> { new() { Id = 0, Question = "   " } }
        };

        // Act
        var exception = await Assert.ThrowsAsync<ArgumentException>(() =>
            handler.Handle(command, CancellationToken.None));

        // Assert
        Assert.Equal("Cannot insert a new question with empty text.", exception.Message);
    }

    /// <summary>
    /// Verifies that <see cref="SetQuestionsCommandHandler"/> throws KeyNotFoundException for invalid existing question IDs.
    /// </summary>
    [Fact]
    public async Task SetQuestionsCommandHandler_WhenInvalidExistingQuestionIds_ShouldThrowKeyNotFoundException()
    {
        // Arrange
        var handler = new SetQuestionsCommandHandler(mockDbContext.Object);
        var command = new SetQuestionsCommand
        {
            InterviewId = 1,
            Questions = new List<QuestionUpdateDto>
            {
                new() { Id = 99, Question = "" },
                new() { Id = 88, Question = "Updated Question" }
            }
        };

        // Act
        var exception = await Assert.ThrowsAsync<KeyNotFoundException>(() =>
            handler.Handle(command, CancellationToken.None));

        // Assert
        Assert.Equal("Some question IDs not found: 99, 88", exception.Message);
    }
}
