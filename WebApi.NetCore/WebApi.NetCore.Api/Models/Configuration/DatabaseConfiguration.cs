namespace WebApi.NetCore.Api.Models.Configuration
{
    using WebApi.NetCore.Api.Contracts.Configuration;

    public class DatabaseConfiguration : IDatabaseConfiguration
    {
        /// <summary>
        ///     Gets the name of the collection.
        /// </summary>
        public string CollectionName { get; set; }

        /// <summary>
        ///     Gets the name of the database.
        /// </summary>
        public string DatabaseName { get; set; }
    }
}
