namespace Generic.Base.Api.Transformer
{
    /// <summary>
    ///     Transformer for entries used by atomic services.
    /// </summary>
    /// <typeparam name="TCreateEntry">
    ///     The type that describes the data for creating an entry of type
    ///     <typeparamref name="TEntry" />.
    /// </typeparam>
    /// <typeparam name="TEntry">The type of the entry.</typeparam>
    /// <typeparam name="TUpdateEntry">
    ///     The type that describes the data for updating an entry of type
    ///     <typeparamref name="TEntry" />.
    /// </typeparam>
    public interface IAtomicTransformer<in TCreateEntry, out TEntry, in TUpdateEntry>
    {
        /// <summary>
        ///     Transforms the specified <paramref name="createEntry" /> of type <typeparamref name="TCreateEntry" /> to
        ///     <typeparamref name="TEntry" />.
        /// </summary>
        /// <param name="createEntry">The data for creating an entry of type <typeparamref name="TEntry" />.</param>
        /// <returns>The transformed entry of type <typeparamref name="TEntry" />.</returns>
        TEntry Transform(TCreateEntry createEntry);

        /// <summary>
        ///     Transforms the specified <paramref name="updateEntry" /> of type <typeparamref name="TUpdateEntry" /> to
        ///     <typeparamref name="TEntry" />.
        /// </summary>
        /// <param name="updateEntry">The data for creating an entry of type <typeparamref name="TEntry" />.</param>
        /// <param name="id">The id of the entry to be updated.</param>
        /// <returns>The transformed entry of type <typeparamref name="TEntry" />.</returns>
        TEntry Transform(TUpdateEntry updateEntry, string id);
    }
}
