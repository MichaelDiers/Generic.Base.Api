namespace Generic.Base.Api.Transformer
{
    using Generic.Base.Api.Result;

    /// <summary>
    ///     Transformer for entries used by database providers, atomic services and controllers.
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
    /// <typeparam name="TDatabaseEntry">The type of the database entry.</typeparam>
    /// <typeparam name="TResultEntry">The type of the result entry that is sent to the client.</typeparam>
    public interface ITransformer<in TCreateEntry, TEntry, in TUpdateEntry, TDatabaseEntry, out TResultEntry>
        : IProviderEntryTransformer<TEntry, TDatabaseEntry>,
            IAtomicTransformer<TCreateEntry, TEntry, TUpdateEntry>,
            IControllerTransformer<TEntry, TResultEntry> where TResultEntry : ILinkResult
    {
    }
}
