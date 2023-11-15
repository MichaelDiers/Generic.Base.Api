namespace Generic.Base.Api.Models
{
    /// <summary>
    ///     Describes an error result.
    /// </summary>
    public class ErrorResult
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="ErrorResult" /> class.
        /// </summary>
        /// <param name="error">The error message.</param>
        public ErrorResult(string error)
        {
            this.Error = error;
        }

        /// <summary>
        ///     Gets the error message.
        /// </summary>
        public string Error { get; }
    }
}
