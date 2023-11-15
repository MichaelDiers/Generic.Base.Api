namespace Generic.Base.Api.Result
{
    /// <summary>
    ///     Describes an operation and the operation url.
    /// </summary>
    public interface ILink
    {
        /// <summary>
        ///     Gets the url of the operation.
        /// </summary>
        string Url { get; }

        /// <summary>
        ///     Gets the type of the operation.
        /// </summary>
        /// <seealso cref="Result.Urn" />
        string Urn { get; }
    }
}
