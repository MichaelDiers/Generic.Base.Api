namespace WebApi.NetCore.Api.Exceptions
{
    /// <summary>
    ///     Exception thrown if the request is invalid.
    /// </summary>
    /// <seealso cref="System.Exception" />
    public class ConflictException : Exception
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="ConflictException" /> class.
        /// </summary>
        public ConflictException()
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="ConflictException" /> class.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public ConflictException(string message)
            : base(message)
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="ConflictException" /> class.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        /// <param name="inner">The inner exception.</param>
        public ConflictException(string message, Exception inner)
            : base(
                message,
                inner)
        {
        }
    }
}
