namespace Generic.Base.Api.MongoDb.AuthServices
{
    /// <summary>
    ///     The default configuration of a mongo db.
    /// </summary>
    /// <seealso cref="Generic.Base.Api.MongoDb.AuthServices.IInvitationDatabaseConfiguration" />
    /// <seealso cref="Generic.Base.Api.MongoDb.AuthServices.ITokenEntryDatabaseConfiguration" />
    /// <seealso cref="Generic.Base.Api.MongoDb.AuthServices.IUserDatabaseConfiguration" />
    internal class MongoDbDatabaseConfiguration
        : IInvitationDatabaseConfiguration, ITokenEntryDatabaseConfiguration, IUserDatabaseConfiguration
    {
        /// <summary>
        ///     The invitation database configuration name.
        /// </summary>
        public const string InvitationDatabaseConfiguration = "InvitationDatabase";

        /// <summary>
        ///     The token entry database configuration name.
        /// </summary>
        public const string TokenEntryDatabaseConfiguration = "TokenEntryDatabase";

        /// <summary>
        ///     The user database configuration name.
        /// </summary>
        public const string UserDatabaseConfiguration = "UserDatabase";

        /// <summary>
        ///     Initializes a new instance of the <see cref="MongoDbDatabaseConfiguration" /> class.
        /// </summary>
        /// <param name="collectionName">Name of the collection.</param>
        /// <param name="databaseName">Name of the database.</param>
        public MongoDbDatabaseConfiguration(string collectionName, string databaseName)
        {
            this.CollectionName = collectionName;
            this.DatabaseName = databaseName;
        }

        /// <summary>
        ///     Gets the name of the collection.
        /// </summary>
        public string CollectionName { get; }

        /// <summary>
        ///     Gets the name of the database.
        /// </summary>
        public string DatabaseName { get; }
    }
}
