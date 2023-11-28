namespace Generic.Base.Api.AuthServices.TokenService
{
    using Generic.Base.Api.Models;
    using Generic.Base.Api.Transformer;

    /// <summary>
    ///     Transformer of token entries.
    /// </summary>
    internal class TokenEntryTransformer
        : IControllerTransformer<TokenEntry, ResultTokenEntry>, IAtomicTransformer<TokenEntry, TokenEntry, TokenEntry>
    {
        /// <summary>
        ///     Transforms the specified <paramref name="createEntry" /> of type <see cref="TokenEntry" /> to
        ///     <see cref="TokenEntry" />.
        /// </summary>
        /// <param name="createEntry">The data for creating an entry of type <see cref="TokenEntry" />.</param>
        /// <returns>The transformed entry of type <see cref="TokenEntry" />.</returns>
        public TokenEntry Transform(TokenEntry createEntry)
        {
            return createEntry;
        }

        /// <summary>
        ///     Transforms the specified <paramref name="updateEntry" /> of type <see cref="TokenEntry" /> to
        ///     <see cref="TokenEntry" />.
        /// </summary>
        /// <param name="updateEntry">The data for creating an entry of type <see cref="TokenEntry" />.</param>
        /// <param name="id">The id of the entry to be updated.</param>
        /// <returns>The transformed entry of type <see cref="TokenEntry" />.</returns>
        public TokenEntry Transform(TokenEntry updateEntry, string id)
        {
            return updateEntry;
        }

        /// <summary>
        ///     Transforms the specified entry of <see cref="TokenEntry" /> to <see cref="ResultTokenEntry" />.
        /// </summary>
        /// <param name="entry">The data for creating an instance of <see cref="ResultTokenEntry" />.</param>
        /// <param name="links">The urn that describe the supported operations on the result entry.</param>
        /// <returns>The transformed entry of type <see cref="ResultTokenEntry" />.</returns>
        public ResultTokenEntry Transform(TokenEntry entry, IEnumerable<Link> links)
        {
            return new ResultTokenEntry(
                entry.Id,
                entry.UserId,
                entry.ValidUntil,
                links);
        }
    }
}
