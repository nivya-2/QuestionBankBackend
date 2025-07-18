using FluentAssertions;
using QuestionBank.Application.Features.InterviewManagement;
using static QuestionBank.Shared.QuestionBankEnums;

namespace QuestionBankTest.Features.InterviewManagement.Validators;

/// <summary>
/// Unit tests for <see cref="UpdateInterviewCommandValidator"/>.
/// </summary>
/// <remarks>
/// Validates InterviewId, Role, Experience, InterviewStatus, and SkillIds.
/// </remarks>
public class UpdateInterviewCommandValidatorTests
{
    /// <summary>
    /// The validator instance used to validate <see cref="UpdateInterviewCommand"/>.
    /// </summary>
    private readonly UpdateInterviewCommandValidator _validator;

    /// <summary>
    /// Initializes the test class with the validator instance.
    /// </summary>
    public UpdateInterviewCommandValidatorTests()
    {
        _validator = new UpdateInterviewCommandValidator();
    }

    /// <summary>
    /// Ensures validation passes for a valid <see cref="UpdateInterviewCommand"/>.
    /// </summary>
    [Fact(DisplayName = "Should pass validation for valid command")]
    public void UpdateInterviewCommand_WhenValid_ShouldPassValidation()
    {
        // Arrange
        var command = new UpdateInterviewCommand
        {
            InterviewId = 1,
            Role = "Software Engineer",
            Experience = 2.5m,
            InterviewStatus = InterviewStatus.Draft,
            SkillIds = new List<int> { 1, 2, 3 }
        };


        // Act
        var result = _validator.Validate(command);

        // Assert
        result.IsValid.Should().BeTrue();
    }

    /// <summary>
    /// Ensures validation fails when InterviewId is 0 or negative.
    /// </summary>
    /// <param name="interviewId">Invalid InterviewId to test.</param>
    [Theory(DisplayName = "When InterviewId is zero or negative, should fail validation")]
    [InlineData(0)]
    [InlineData(-1)]
    public void UpdateInterviewCommand_WhenInterviewIdIsZeroOrNegative_ShouldFailValidation(int interviewId)
    {
        // Arrange
        var command = new UpdateInterviewCommand { InterviewId = interviewId };

        // Act
        var result = _validator.Validate(command);

        // Assert
        result.Errors.Should().ContainSingle(e => e.PropertyName == nameof(command.InterviewId));
    }

    /// <summary>
    /// Ensures validation fails for null, empty, or overly long Role values.
    /// </summary>
    /// <param name="role">Role value to test.</param>
    [Theory(DisplayName = "Should fail for null, empty or overly long Role")]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData("A role that exceeds one hundred characters should absolutely faidddl validation because it's too long.")]
    public void UpdateInterviewCommand_WhenRoleIsNullEmptyOrTooLong_ShouldFailValidation(string? role)
    {
        // Arrange
        var command = new UpdateInterviewCommand { Role = role };

        // Act
        var result = _validator.Validate(command);

        // Assert
        result.Errors.Should().Contain(e => e.PropertyName == nameof(command.Role));
    }

    /// <summary>
    /// Ensures validation fails for negative Experience values.
    /// </summary>
    /// <param name="experience">Experience value to test.</param>
    [Theory(DisplayName = "Should fail for negative experience")]
    [InlineData(-1)]
    [InlineData(-10)]
    public void UpdateInterviewCommand_WhenExperienceIsNegative_ShouldFailValidation(decimal experience)
    {
        // Arrange
        var command = new UpdateInterviewCommand { Experience = experience };

        // Act
        var result = _validator.Validate(command);

        // Assert
        result.Errors.Should().Contain(e => e.PropertyName == nameof(command.Experience));
    }

    /// <summary>
    /// Ensures validation fails for undefined InterviewStatus enum values.
    /// </summary>
    [Fact(DisplayName = "Should fail for invalid InterviewStatus enum value")]
    public void UpdateInterviewCommand_WhenInterviewStatusIsInvalid_ShouldFailValidation()
    {
        // Arrange
        var command = new UpdateInterviewCommand
        {
            InterviewStatus = (InterviewStatus)999 // out of enum range
        };

        // Act
        var result = _validator.Validate(command);

        // Assert
        result.Errors.Should().Contain(e => e.PropertyName == nameof(command.InterviewStatus));
    }

    /// <summary>
    /// Ensures validation fails when SkillIds contains duplicates.
    /// </summary>
    [Fact(DisplayName = "Should fail when SkillIds contains duplicate values")]
    public void UpdateInterviewCommand_WhenSkillIdsContainDuplicates_ShouldFailValidation()
    {
        // Arrange
        var command = new UpdateInterviewCommand { SkillIds = new List<int> { 1, 2, 2 } };

        // Act
        var result = _validator.Validate(command);

        // Assert
        result.Errors.Should().Contain(e => e.PropertyName == nameof(command.SkillIds))
            .And.Contain(e => e.ErrorMessage.Contains("Duplicate"));
    }

    /// <summary>
    /// Ensures validation fails when SkillIds contains 0 or negative values.
    /// </summary>
    [Fact(DisplayName = "Should fail when SkillIds contains zero or negative values")]
    public void UpdateInterviewCommand_WhenSkillIdsContainZeroOrNegativeValues_ShouldFailValidation()
    {
        // Arrange
        var command = new UpdateInterviewCommand { SkillIds = new List<int> { 1, 0, -2 } };

        // Act
        var result = _validator.Validate(command);

        // Assert
        result.Errors.Should().Contain(e => e.PropertyName == nameof(command.SkillIds));
    }
}
