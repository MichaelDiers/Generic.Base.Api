namespace Generic.Base.Api.Exceptions
{
    /// <summary>
    ///     Exception thrown if an item does not exists.
    /// </summary>
    /// <seealso cref="Exception" />
    public class UnauthorizedException : Exception
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="UnauthorizedException" /> class.
        /// </summary>
        public UnauthorizedException()
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="UnauthorizedException" /> class.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public UnauthorizedException(string message)
            : base(message)
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="UnauthorizedException" /> class.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        /// <param name="inner">The inner exception.</param>
        public UnauthorizedException(string message, Exception inner)
            : base(
                message,
                inner)
        {
        }
    }
}
