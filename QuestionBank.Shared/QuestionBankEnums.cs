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
        /// A newly created interview that hasn't been edited yet (Next button not clicked).
        /// </summary>
        New,

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
