namespace Generic.Base.Api.Models
{
    /// <summary>
    ///     Describes an operation and the operation url.
    /// </summary>
    public interface ILink
    {
        /// <summary>
        ///     Gets the url of the operation.
        /// </summary>
        /// <example>/Item/720007f1-b9ce-4448-9a38-2cd93b99f254</example>
        string Url { get; }

        /// <summary>
        ///     Gets the type of the operation.
        /// </summary>
        /// <seealso cref="Models.Urn" />
        /// <example>urn:Create</example>
        string Urn { get; }
    }
}
