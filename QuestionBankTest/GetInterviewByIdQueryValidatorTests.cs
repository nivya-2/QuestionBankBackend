using FluentAssertions;
using QuestionBank.Application.Features.InterviewManagement;

namespace QuestionBankTest;

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
        var result = validator.Validate(query);

        // Assert
        result.IsValid.Should().BeTrue("because InterviewId is a positive non-zero value");
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
        var result = validator.Validate(query);

        // Assert
        result.IsValid.Should().BeFalse("because InterviewId cannot be zero");
        result.Errors.Should().ContainSingle(e => e.PropertyName == nameof(query.InterviewId));
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
        var result = validator.Validate(query);

        // Assert
        result.IsValid.Should().BeFalse("because InterviewId cannot be negative");
        result.Errors.Should().ContainSingle(e => e.PropertyName == nameof(query.InterviewId));
    }
}
