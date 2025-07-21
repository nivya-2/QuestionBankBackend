using FluentAssertions;
using QuestionBank.Application.Features.InterviewManagement;
using FluentValidation.TestHelper;

namespace QuestionBankTest.Features.InterviewManagement.Validators;

/// <summary>
/// Unit test class for <see cref="GetInterviewByIdQueryValidator"/>.
/// Verifies validation behavior for querying a specific interview by ID.
/// </summary>
public class GetInterviewByIdQueryValidatorTests
{
    /// <summary>
    /// Verifies that the query passes when a positive interview ID is provided.
    /// </summary>
    [Fact]
    public void GetInterviewByIdQuery_WhenIdIsValid_ShouldPassValidation()
    {
        // Arrange
        var validator = new GetInterviewByIdQueryValidator();
        var query = new GetInterviewByIdQuery { InterviewId = 5 };

        // Act
        var result = validator.TestValidate(query);

        // Assert
        result.IsValid.Should().BeTrue();
    }

    /// <summary>
    /// Verifies that the query fails when the interview ID is zero.
    /// </summary>
    [Fact]
    public void GetInterviewByIdQuery_WhenIdIsZero_ShouldFailValidation()
    {
        // Arrange
        var validator = new GetInterviewByIdQueryValidator();
        var query = new GetInterviewByIdQuery { InterviewId = 0 };

        // Act
        var result = validator.TestValidate(query);

        // Assert
        result.IsValid.Should().BeFalse();
        result.ShouldHaveValidationErrorFor(q => q.InterviewId);
    }

    /// <summary>
    /// Verifies that the query fails when the interview ID is negative.
    /// </summary>
    [Fact]
    public void GetInterviewByIdQuery_WhenIdIsNegative_ShouldFailValidation()
    {
        // Arrange
        var validator = new GetInterviewByIdQueryValidator();
        var query = new GetInterviewByIdQuery { InterviewId = -10 };

        // Act
        var result = validator.TestValidate(query);

        // Assert
        result.IsValid.Should().BeFalse();
        result.ShouldHaveValidationErrorFor(q => q.InterviewId);
    }
}
