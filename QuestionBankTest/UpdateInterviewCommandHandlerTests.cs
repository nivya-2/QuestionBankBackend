using FluentAssertions;
using MockQueryable.Moq;
using Moq;
using QuestionBank.Application.Contracts.Persistence;
using QuestionBank.Domain.Entities;
using static QuestionBank.Shared.QuestionBankEnums;

namespace QuestionBankTest;

/// <summary>
/// Unit test class for <see cref="UpdateInterviewCommandHandler"/>.
/// Verifies correct update behavior for valid, invalid, and edge-case scenarios.
/// </summary>
public class UpdateInterviewCommandHandlerTests
{
    /// <summary>
    /// Mocked instance of <see cref="IQuestionBankDbContext"/> to simulate database operations.
    /// </summary>
    private readonly Mock<IQuestionBankDbContext> mockDbContext;

    /// <summary>
    /// Initializes a new instance of the <see cref="UpdateInterviewCommandHandlerTests"/> class.
    /// Seeds mock database with interview and skills.
    /// </summary>
    public UpdateInterviewCommandHandlerTests()
    {
        //Arrange
        mockDbContext = new Mock<IQuestionBankDbContext>();

        var skills = new List<Skill>
        {
            new Skill { Id = 1, Name = "C#" },
            new Skill { Id = 2, Name = "Angular" },
            new Skill { Id = 3, Name = "SQL" }
        };

        var interviewSkills = new List<InterviewSkill>
        {
            new InterviewSkill { InterviewId = 1, SkillId = 1 },
            new InterviewSkill { InterviewId = 1, SkillId = 2 }
        };

        var interviews = new List<Interview>
        {
            new Interview
            {
                Id = 1,
                Role = "Old Role",
                Status = InterviewStatus.Draft,
                Experience = 1.5m,
                InterviewSkills = interviewSkills
            }
        };

        mockDbContext.Setup(db => db.Interviews)
            .Returns(interviews.AsQueryable().BuildMockDbSet().Object);

        mockDbContext.Setup(db => db.InterviewSkills.RemoveRange(It.IsAny<IEnumerable<InterviewSkill>>()))
            .Callback<IEnumerable<InterviewSkill>>(skills => interviewSkills.RemoveAll(s => skills.Contains(s)));

        mockDbContext.Setup(db => db.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);
    }

    /// <summary>
    /// Verifies that <see cref="UpdateInterviewCommandHandler"/> updates the interview and modifies its skills.
    /// </summary>
    [Fact]
    public async Task UpdateInterviewCommandHandler_WhenValidRequest_ShouldUpdateInterviewAndSkills()
    {
        // Arrange
        var handler = new UpdateInterviewCommandHandler(mockDbContext.Object);
        var command = new UpdateInterviewCommand
        {
            InterviewId = 1,
            Role = "Updated Role",
            InterviewStatus = InterviewStatus.Submitted,
            Experience = 3.5m,
            SkillIds = new List<int> { 1, 3 }
        };

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().BeTrue();
        var updatedInterview = mockDbContext.Object.Interviews.First(i => i.Id == 1);
        updatedInterview.Role.Should().Be("Updated Role");
        updatedInterview.Status.Should().Be(InterviewStatus.Submitted);
        updatedInterview.Experience.Should().Be(3.5m);
        updatedInterview.InterviewSkills.Select(s => s.SkillId).Should().BeEquivalentTo(new List<int> { 1, 3 });
    }

    /// <summary>
    /// Verifies that <see cref="UpdateInterviewCommandHandler"/> throws KeyNotFoundException when interview is not found.
    /// </summary>
    [Fact]
    public async Task UpdateInterviewCommandHandler_WhenInterviewNotFound_ShouldThrowKeyNotFoundException()
    {
        // Arrange
        var handler = new UpdateInterviewCommandHandler(mockDbContext.Object);
        var command = new UpdateInterviewCommand
        {
            InterviewId = 99, 
            Role = "Any",
            InterviewStatus = InterviewStatus.Draft,
            Experience = 2,
            SkillIds = new List<int> { 1 }
        };

        // Act
        var act = async () => await handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<KeyNotFoundException>()
            .WithMessage("Interview with ID 99 not found.");
    }

    /// <summary>
    /// Verifies that <see cref="UpdateInterviewCommandHandler"/> throws ArgumentException for invalid status enum.
    /// </summary>
    [Fact]
    public async Task UpdateInterviewCommandHandler_WhenStatusInvalid_ShouldThrowArgumentException()
    {
        // Arrange
        var handler = new UpdateInterviewCommandHandler(mockDbContext.Object);
        var command = new UpdateInterviewCommand
        {
            InterviewId = 1,
            Role = "Role",
            InterviewStatus = (InterviewStatus)999, // Invalid enum
            Experience = 3,
            SkillIds = new List<int> { 1 }
        };

        // Act
        var act = async () => await handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<ArgumentException>()
            .WithMessage("Invalid interview status value.");
    }
}
