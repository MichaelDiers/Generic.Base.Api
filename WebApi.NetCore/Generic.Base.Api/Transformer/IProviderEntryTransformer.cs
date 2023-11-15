namespace Generic.Base.Api.Transformer
{
    /// <summary>
    ///     Transformer for entries used by database providers.
    /// </summary>
    /// <typeparam name="TEntry">The type of the entry.</typeparam>
    /// <typeparam name="TDatabaseEntry">The type of the database entry.</typeparam>
    public interface IProviderEntryTransformer<TEntry, TDatabaseEntry>
    {
        /// <summary>
        ///     Transforms the specified entry of type <typeparamref name="TEntry" /> to <typeparamref name="TDatabaseEntry" />.
        /// </summary>
        /// <param name="entry">The data for creating an instance of type <typeparamref name="TDatabaseEntry" />.</param>
        /// <returns>The transformed entry of type <typeparamref name="TDatabaseEntry" />.</returns>
        TDatabaseEntry Transform(TEntry entry);

        /// <summary>
        ///     Transforms the specified database entry to instance of <typeparamref name="TEntry" />.
        /// </summary>
        /// <param name="databaseEntry">The data for creating an instance of <typeparamref name="TEntry" />.</param>
        /// <returns>The transformed entry of type <typeparamref name="TEntry" />.</returns>
        TEntry Transform(TDatabaseEntry databaseEntry);
    }
}
