namespace Generic.Base.Api.Transformer
{
    using Generic.Base.Api.Models;

    /// <summary>
    ///     Transformer for entries used by controllers.
    /// </summary>
    /// <typeparam name="TEntry">The type of the entry.</typeparam>
    /// <typeparam name="TResultEntry">The type of the result entry that is sent to the client.</typeparam>
    public interface IControllerTransformer<in TEntry, out TResultEntry> where TResultEntry : ILinkResult
    {
        /// <summary>
        ///     Transforms the specified entry of <typeparamref name="TEntry" /> to <typeparamref name="TResultEntry" />.
        /// </summary>
        /// <param name="entry">The data for creating an instance of <typeparamref name="TResultEntry" />.</param>
        /// <param name="links">The urn that describe the supported operations on the result entry.</param>
        /// <returns>The transformed entry of type <typeparamref name="TResultEntry" />.</returns>
        TResultEntry Transform(TEntry entry, IEnumerable<Link> links);
    }
}
