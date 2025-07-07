namespace QuestionBank.Shared;

/// <summary>
/// Contains enums used across the QuestionBank application.
/// </summary>
public class QuestionBankEnums
{
    /// <summary>
    /// Represents the status of an interview.
    /// </summary>
    public enum InterviewStatus
    {

        /// <summary>
        /// An interview that is in progress but not finalized (Next button clicked).
        /// </summary>
        Draft,

        /// <summary>
        /// A finalized interview that has been submitted (Submit button clicked).
        /// </summary>
        Submitted
    }
}
