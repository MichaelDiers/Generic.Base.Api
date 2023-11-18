namespace Generic.Base.Api.Models
{
    /// <summary>
    ///     Describes the result of an api request.
    /// </summary>
    public interface ILinkResult
    {
        /// <summary>
        ///     Gets the links to available operations.
        /// </summary>
        IEnumerable<ILink> Links { get; }
    }
}
