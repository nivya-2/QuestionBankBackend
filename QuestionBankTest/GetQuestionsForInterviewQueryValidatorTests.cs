using FluentAssertions;
using QuestionBank.Application.Features.QuestionManagement;
using FluentValidation.TestHelper;
using QuestionBank.Application.Features.InterviewManagement;

namespace QuestionBankTest.Features.QuestionManagement.Validators;

/// <summary>
/// Unit test class for <see cref="GetQuestionsForInterviewQueryValidator"/>.
/// Verifies validation behavior for querying a specific interview by ID.
/// </summary>
public class GetQuestionsForInterviewQueryValidatorTests
{
    /// <summary>
    /// Verifies that the query passes when a positive interview ID is provided.
    /// </summary>
    [Fact]
    public void GetQuestionsForInterviewQueryValidator_WhenIdIsValid_ShouldPassValidation()
    {
        // Arrange
        var validator = new GetQuestionsForInterviewQueryValidator();
        var query = new GetQuestionsForInterviewQuery { InterviewId = 5 };

        // Act
        var result = validator.TestValidate(query);

        // Assert
        result.IsValid.Should().BeTrue();
    }

    /// <summary>
    /// Verifies that the query fails when the interview ID is zero.
    /// </summary>
    [Fact]
    public void GetQuestionsForInterviewQueryValidator_WhenIdIsZero_ShouldFailValidation()
    {
        // Arrange
        var validator = new GetQuestionsForInterviewQueryValidator();
        var query = new GetQuestionsForInterviewQuery { InterviewId = 0 };

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
    public void GetQuestionsForInterviewQueryValidator_WhenIdIsNegative_ShouldFailValidation()
    {
        // Arrange
        var validator = new GetQuestionsForInterviewQueryValidator();
        var query = new GetQuestionsForInterviewQuery { InterviewId = -10 };

        // Act
        var result = validator.TestValidate(query);

        // Assert
        result.IsValid.Should().BeFalse();
        result.ShouldHaveValidationErrorFor(q => q.InterviewId);
    }
}
