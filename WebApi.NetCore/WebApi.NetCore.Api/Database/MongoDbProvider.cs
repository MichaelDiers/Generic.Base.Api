namespace WebApi.NetCore.Api.Database
{
    using System.Linq.Expressions;
    using MongoDB.Driver;
    using WebApi.NetCore.Api.Contracts.Configuration;
    using WebApi.NetCore.Api.Contracts.Database;
    using WebApi.NetCore.Api.Exceptions;

    /// <summary>
    ///     An generic class for mongo db based providers.
    /// </summary>
    /// <typeparam name="TEntry">The type of the entry.</typeparam>
    /// <typeparam name="TDatabaseEntry">The type of the database entry.</typeparam>
    /// <typeparam name="TDatabaseConfiguration">The type of the database configuration.</typeparam>
    public abstract class
        MongoDbProvider<TEntry, TDatabaseEntry, TDatabaseConfiguration> : IProvider<TEntry, IClientSessionHandle>
        where TDatabaseConfiguration : IDatabaseConfiguration where TDatabaseEntry : IDatabaseEntry
    {
        private readonly IMongoCollection<TDatabaseEntry> collection;
        private readonly Func<TEntry, TDatabaseEntry> toDatabaseEntry;
        private readonly Func<TDatabaseEntry, TEntry> toEntry;

        protected MongoDbProvider(
            IMongoClient mongoClient,
            TDatabaseConfiguration databaseConfiguration,
            Func<TEntry, TDatabaseEntry> toDatabaseEntry,
            Func<TDatabaseEntry, TEntry> toEntry
        )
        {
            this.toDatabaseEntry = toDatabaseEntry;
            this.toEntry = toEntry;
            this.collection = mongoClient.GetDatabase(databaseConfiguration.DatabaseName)
                .GetCollection<TDatabaseEntry>(databaseConfiguration.CollectionName);
        }

        /// <summary>
        ///     Creates the specified entry.
        /// </summary>
        /// <param name="entry">The entry.</param>
        /// <param name="cancellationToken">Indicates that the start process has been aborted.</param>
        /// <param name="transactionHandle">Handle for database transactions.</param>
        /// <returns>A <see cref="Task" /> whose result indicates success.</returns>
        public async Task CreateAsync(
            TEntry entry,
            CancellationToken cancellationToken,
            ITransactionHandle<IClientSessionHandle> transactionHandle
        )
        {
            var databaseEntry = this.toDatabaseEntry(entry);
            try
            {
                await this.collection.InsertOneAsync(
                    transactionHandle.ClientSessionHandle,
                    databaseEntry,
                    cancellationToken: cancellationToken);
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
        ///     Delete an entry by its id used in the application.
        /// </summary>
        /// <param name="applicationId">The id of the entry used in the application.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <param name="transactionHandle">The transaction handle.</param>
        /// <returns>A <see cref="Task" /> whose result indicates success.</returns>
        public async Task DeleteAsync(
            string applicationId,
            CancellationToken cancellationToken,
            ITransactionHandle<IClientSessionHandle> transactionHandle
        )
        {
            var result = await this.collection.DeleteOneAsync(
                transactionHandle.ClientSessionHandle,
                doc => string.Equals(
                    doc.ApplicationId,
                    applicationId,
                    StringComparison.OrdinalIgnoreCase),
                cancellationToken: cancellationToken);
            if (!result.IsAcknowledged || result.DeletedCount == 0)
            {
                throw new NotFoundException($"Cannot delete {nameof(TDatabaseEntry)} due to unknown id.");
            }
        }

        /// <summary>
        ///     Read all entries from the database.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <param name="transactionHandle">The transaction handle.</param>
        /// <returns>A <see cref="Task{T}" /> whose result is a list of all entries.</returns>
        public async Task<IEnumerable<TEntry>> ReadAsync(
            CancellationToken cancellationToken,
            ITransactionHandle<IClientSessionHandle> transactionHandle
        )
        {
            var cursor = await this.collection.FindAsync(
                transactionHandle.ClientSessionHandle,
                doc => true,
                cancellationToken: cancellationToken);
            var result = await cursor.ToListAsync(cancellationToken);
            return result.Select(this.toEntry).ToArray();
        }

        /// <summary>
        ///     Read an entry by its id used in the application.
        /// </summary>
        /// <param name="applicationId">The id of the entry used in the application.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <param name="transactionHandle">The transaction handle.</param>
        /// <returns>A <see cref="Task{T}" /> whose result is the found entry.</returns>
        public async Task<TEntry> ReadByIdAsync(
            string applicationId,
            CancellationToken cancellationToken,
            ITransactionHandle<IClientSessionHandle> transactionHandle
        )
        {
            var result = await this.collection.FindAsync(
                transactionHandle.ClientSessionHandle,
                doc => string.Equals(
                    doc.ApplicationId,
                    applicationId,
                    StringComparison.OrdinalIgnoreCase),
                cancellationToken: cancellationToken);
            var entry = result.FirstOrDefault(cancellationToken);
            if (entry is null)
            {
                throw new NotFoundException($"Cannot read {nameof(TDatabaseEntry)} entry due to its unknown id.");
            }

            return this.toEntry(entry);
        }

        /// <summary>
        ///     Updates the specified entry.
        /// </summary>
        /// <param name="entry">The entry to be replaced.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <param name="transactionHandle">The transaction handle.</param>
        /// <returns>A <see cref="Task" /> whose result indicates success.</returns>
        public abstract Task UpdateAsync(
            TEntry entry,
            CancellationToken cancellationToken,
            ITransactionHandle<IClientSessionHandle> transactionHandle
        );

        /// <summary>
        ///     Updates the specified entry.
        /// </summary>
        /// <param name="filter">The filter definition.</param>
        /// <param name="update">The update definition.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <param name="transactionHandle">The transaction handle.</param>
        /// <returns>A <see cref="Task" /> whose result indicates success.</returns>
        protected async Task UpdateAsync(
            Expression<Func<TDatabaseEntry, bool>> filter,
            UpdateDefinition<TDatabaseEntry> update,
            CancellationToken cancellationToken,
            ITransactionHandle<IClientSessionHandle> transactionHandle
        )
        {
            try
            {
                var result = await this.collection.UpdateOneAsync(
                    transactionHandle.ClientSessionHandle,
                    filter,
                    update,
                    cancellationToken: cancellationToken);
                if (!result.IsAcknowledged || result.MatchedCount == 0)
                {
                    throw new NotFoundException(
                        $"Cannot update {nameof(TDatabaseEntry)} entry: Filter does not match.");
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
