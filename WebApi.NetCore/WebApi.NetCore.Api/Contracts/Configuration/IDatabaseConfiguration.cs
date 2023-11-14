namespace WebApi.NetCore.Api.Contracts.Configuration
{
    /// <summary>
    ///     The configuration of the database.
    /// </summary>
    public interface IDatabaseConfiguration
    {
        /// <summary>
        ///     Gets the name of the collection.
        /// </summary>
        string CollectionName { get; }

        /// <summary>
        ///     Gets the name of the database.
        /// </summary>
        string DatabaseName { get; }
    }
}
