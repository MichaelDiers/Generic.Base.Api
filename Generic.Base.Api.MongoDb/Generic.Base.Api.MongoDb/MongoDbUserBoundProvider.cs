namespace Generic.Base.Api.MongoDb
{
    using Generic.Base.Api.Database;
    using Generic.Base.Api.Exceptions;
    using Generic.Base.Api.Transformer;
    using MongoDB.Driver;

    /// <summary>
    ///     Generic mongo db provider.
    /// </summary>
    /// <typeparam name="TEntry">The type of the entry.</typeparam>
    /// <typeparam name="TDatabaseEntry">The type of the database entry.</typeparam>
    /// <typeparam name="TDatabaseConfiguration">The type of the database configuration.</typeparam>
    internal class
        MongoDbUserBoundProvider<TEntry, TDatabaseEntry, TDatabaseConfiguration> : IDatabaseUserBoundProvider<TEntry>
        where TDatabaseConfiguration : IDatabaseConfiguration where TDatabaseEntry : IUserBoundEntry
    {
        /// <summary>
        ///     The collection for <typeparamref name="TDatabaseEntry" />.
        /// </summary>
        private readonly IMongoCollection<TDatabaseEntry> collection;

        /// <summary>
        ///     The transformer from <typeparamref name="TEntry" /> to <typeparamref name="TDatabaseEntry" /> and vice versa.
        /// </summary>
        private readonly IProviderEntryTransformer<TEntry, TDatabaseEntry> transformer;

        /// <summary>
        ///     Initializes a new instance of the <see cref="MongoDbProvider{TEntry, TDatabaseEntry, TDatabaseConfiguration}" />
        ///     class.
        /// </summary>
        /// <param name="mongoClient">The mongo client.</param>
        /// <param name="databaseConfiguration">The database configuration.</param>
        /// <param name="transformer">
        ///     The transformer from <typeparamref name="TEntry" /> to <typeparamref name="TDatabaseEntry" />
        ///     and vice versa.
        /// </param>
        public MongoDbUserBoundProvider(
            IMongoClient mongoClient,
            TDatabaseConfiguration databaseConfiguration,
            IProviderEntryTransformer<TEntry, TDatabaseEntry> transformer
        )
        {
            this.transformer = transformer;
            this.collection = mongoClient.GetDatabase(databaseConfiguration.DatabaseName)
                .GetCollection<TDatabaseEntry>(databaseConfiguration.CollectionName);
        }

        /// <summary>
        ///     Creates the specified entry.
        /// </summary>
        /// <param name="entry">The entry to ve created.</param>
        /// <param name="cancellationToken">Indicates that the start process has been aborted.</param>
        /// <param name="transactionHandle">Handle for database transactions.</param>
        /// <returns>A <see cref="Task{T}" /> whose result is the created entry.</returns>
        public async Task<TEntry> CreateAsync(
            TEntry entry,
            CancellationToken cancellationToken,
            ITransactionHandle<IClientSessionHandle> transactionHandle
        )
        {
            var databaseEntry = this.transformer.Transform(entry);
            try
            {
                await this.collection.InsertOneAsync(
                    transactionHandle.ClientSessionHandle,
                    databaseEntry,
                    cancellationToken: cancellationToken);
                return this.transformer.Transform(databaseEntry);
            }
            catch (MongoWriteException e) when (e.WriteError.Code == 11000)
            {
                throw new ConflictException(
                    $"Cannot insert {nameof(databaseEntry)} entry: Entry already exists.",
                    e);
            }
            catch (MongoWriteException e) when (e.WriteError.Code == 121)
            {
                throw new BadRequestException(
                    $"Cannot insert {nameof(databaseEntry)} entry due to validation failure.",
                    e);
            }
        }

        /// <summary>
        ///     Delete an entry by its id.
        /// </summary>
        /// <param name="userId">The id of the owner.</param>
        /// <param name="entryId">The id of the entry.</param>
        /// <param name="cancellationToken">Indicates that the start process has been aborted.</param>
        /// <param name="transactionHandle">The database transaction handle.</param>
        /// <returns>A <see cref="Task" /> whose result indicates success.</returns>
        public async Task DeleteAsync(
            string userId,
            string entryId,
            CancellationToken cancellationToken,
            ITransactionHandle<IClientSessionHandle> transactionHandle
        )
        {
            if (string.IsNullOrWhiteSpace(entryId))
            {
                throw new ArgumentNullException(nameof(entryId));
            }

            if (string.IsNullOrWhiteSpace(userId))
            {
                throw new ArgumentNullException(userId);
            }

            var result = await this.collection.DeleteOneAsync(
                transactionHandle.ClientSessionHandle,
                doc => string.Equals(
                           doc.UserId,
                           userId,
                           StringComparison.OrdinalIgnoreCase) &&
                       string.Equals(
                           doc.Id,
                           entryId,
                           StringComparison.OrdinalIgnoreCase),
                cancellationToken: cancellationToken);
            if (!result.IsAcknowledged || result.DeletedCount == 0)
            {
                throw new NotFoundException($"Cannot delete {nameof(TDatabaseEntry)} due to unknown id.");
            }
        }

        /// <summary>
        ///     Read all entries from the database provider.
        /// </summary>
        /// <param name="userId">The id of the owner.</param>
        /// <param name="cancellationToken">Indicates that the start process has been aborted.</param>
        /// <param name="transactionHandle">The database transaction handle.</param>
        /// <returns>A <see cref="Task{T}" /> whose result is a list of all entries.</returns>
        public async Task<IEnumerable<TEntry>> ReadAsync(
            string userId,
            CancellationToken cancellationToken,
            ITransactionHandle<IClientSessionHandle> transactionHandle
        )
        {
            if (string.IsNullOrWhiteSpace(userId))
            {
                throw new ArgumentNullException(nameof(userId));
            }

            var cursor = await this.collection.FindAsync(
                transactionHandle.ClientSessionHandle,
                doc => string.Equals(
                    doc.UserId,
                    userId,
                    StringComparison.OrdinalIgnoreCase),
                cancellationToken: cancellationToken);
            var result = await cursor.ToListAsync(cancellationToken);
            return result.Select(this.transformer.Transform).ToArray();
        }

        /// <summary>
        ///     Read an entry by its id.
        /// </summary>
        /// <param name="userId">The id of the owner.</param>
        /// <param name="entryId">The id of the entry.</param>
        /// <param name="cancellationToken">Indicates that the start process has been aborted.</param>
        /// <param name="transactionHandle">The database transaction handle.</param>
        /// <returns>A <see cref="Task{T}" /> whose result is the found entry.</returns>
        public async Task<TEntry> ReadByIdAsync(
            string userId,
            string entryId,
            CancellationToken cancellationToken,
            ITransactionHandle<IClientSessionHandle> transactionHandle
        )
        {
            if (string.IsNullOrWhiteSpace(entryId))
            {
                throw new ArgumentNullException(nameof(entryId));
            }

            if (string.IsNullOrWhiteSpace(userId))
            {
                throw new ArgumentException(
                    "Value cannot be null or whitespace.",
                    nameof(userId));
            }

            var result = await this.collection.FindAsync(
                transactionHandle.ClientSessionHandle,
                doc => string.Equals(
                           doc.UserId,
                           userId,
                           StringComparison.OrdinalIgnoreCase) &&
                       string.Equals(
                           doc.Id,
                           entryId,
                           StringComparison.OrdinalIgnoreCase),
                cancellationToken: cancellationToken);
            var entry = result.FirstOrDefault(cancellationToken);
            if (entry is null)
            {
                throw new NotFoundException($"Cannot read {nameof(TDatabaseEntry)} entry due to its unknown id.");
            }

            return this.transformer.Transform(entry);
        }

        /// <summary>
        ///     Updates the specified entry.
        /// </summary>
        /// <param name="entry">The entry to be replaced.</param>
        /// <param name="cancellationToken">Indicates that the start process has been aborted.</param>
        /// <param name="transactionHandle">The database transaction handle.</param>
        /// <returns>A <see cref="Task" /> whose result indicates success.</returns>
        public async Task UpdateAsync(
            TEntry entry,
            CancellationToken cancellationToken,
            ITransactionHandle<IClientSessionHandle> transactionHandle
        )
        {
            var databaseEntry = this.transformer.Transform(entry);
            try
            {
                var result = await this.collection.ReplaceOneAsync(
                    transactionHandle.ClientSessionHandle,
                    Builders<TDatabaseEntry>.Filter.And(
                        Builders<TDatabaseEntry>.Filter.Eq(
                            doc => doc.Id,
                            databaseEntry.Id),
                        Builders<TDatabaseEntry>.Filter.Eq(
                            doc => doc.UserId,
                            databaseEntry.UserId)),
                    databaseEntry,
                    new ReplaceOptions
                    {
                        Collation = new Collation(
                            "en",
                            strength: new Optional<CollationStrength?>(CollationStrength.Secondary))
                    },
                    cancellationToken);
                if (!result.IsAcknowledged || result.MatchedCount == 0)
                {
                    throw new NotFoundException(
                        $"Cannot replace {nameof(TDatabaseEntry)} entry: Filter does not match.");
                }
            }
            catch (MongoWriteException e) when (e.WriteError.Code == 121)
            {
                throw new BadRequestException(
                    $"Cannot update {nameof(TDatabaseEntry)} entry due to validation failure.",
                    e);
            }
        }
    }
}
