namespace Generic.Base.Api.MongoDb
{
    using Generic.Base.Api.Database;
    using MongoDB.Driver;

    /// <summary>
    ///     Describes basic crud operations for mongo database providers.
    /// </summary>
    /// <typeparam name="TEntry">The type of the entry.</typeparam>
    public interface IDatabaseProvider<TEntry> : IProvider<TEntry, IClientSessionHandle>
    {
    }
}
