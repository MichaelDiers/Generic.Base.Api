namespace Generic.Base.Api.MongoDb.AuthServices
{
    using Generic.Base.Api.Transformer;

    /// <summary>
    ///     An entry transformer.
    /// </summary>
    /// <typeparam name="TEntry">The type of the entry.</typeparam>
    /// <seealso cref="Generic.Base.Api.Transformer.IProviderEntryTransformer&lt;TEntry, TEntry&gt;" />
    internal class EntryTransformer<TEntry> : IProviderEntryTransformer<TEntry, TEntry>
    {
        /// <summary>
        ///     A simple no transformation operation.
        /// </summary>
        /// <param name="entry">The entry.</param>
        /// <returns>The given <paramref name="entry" />.</returns>
        public TEntry Transform(TEntry entry)
        {
            return entry;
        }
    }
}
