namespace WebApi.NetCore.Api.Exceptions
{
    /// <summary>
    ///     Exception thrown if the request is invalid.
    /// </summary>
    /// <seealso cref="System.Exception" />
    public class NotFoundException : Exception
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="NotFoundException" /> class.
        /// </summary>
        public NotFoundException()
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="NotFoundException" /> class.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public NotFoundException(string message)
            : base(message)
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="NotFoundException" /> class.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        /// <param name="inner">The inner exception.</param>
        public NotFoundException(string message, Exception inner)
            : base(
                message,
                inner)
        {
        }
    }
}
