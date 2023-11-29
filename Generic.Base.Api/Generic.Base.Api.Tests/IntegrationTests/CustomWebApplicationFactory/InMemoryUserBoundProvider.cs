namespace Generic.Base.Api.Tests.IntegrationTests.CustomWebApplicationFactory
{
    using Generic.Base.Api.Database;
    using Generic.Base.Api.Exceptions;

    /// <summary>
    ///     A generic database independent version of a provider.
    /// </summary>
    /// <typeparam name="TEntry">The type of the entry.</typeparam>
    /// <typeparam name="TClientSessionHandle">The type of the client session handle.</typeparam>
    /// <seealso cref="Generic.Base.Api.Database.IProvider&lt;TEntry, TClientSessionHandle&gt;" />
    internal class InMemoryUserBoundProvider<TEntry, TClientSessionHandle>
        : IUserBoundProvider<TEntry, TClientSessionHandle> where TEntry : IUserBoundEntry
    {
        /// <summary>
        ///     The in-memory database.
        /// </summary>
        private readonly IDictionary<string, IDictionary<string, TEntry>> database =
            new Dictionary<string, IDictionary<string, TEntry>>();

        /// <summary>
        ///     Creates the specified entry.
        /// </summary>
        /// <param name="entry">The entry to ve created.</param>
        /// <param name="cancellationToken">Indicates that the start process has been aborted.</param>
        /// <param name="transactionHandle">Handle for database transactions.</param>
        /// <returns>A <see cref="Task{T}" /> whose result is the created entry.</returns>
        public Task<TEntry> CreateAsync(
            TEntry entry,
            CancellationToken cancellationToken,
            ITransactionHandle<TClientSessionHandle> transactionHandle
        )
        {
            if (!this.database.TryGetValue(
                    entry.UserId,
                    out var userDictionary))
            {
                userDictionary = new Dictionary<string, TEntry>();
                this.database[entry.UserId] = userDictionary;
            }

            if (!userDictionary.TryGetValue(
                    entry.Id,
                    out _))
            {
                userDictionary[entry.Id] = entry;
                return Task.FromResult(entry);
            }

            throw new ConflictException();
        }

        /// <summary>
        ///     Delete an entry by its id.
        /// </summary>
        /// <param name="userId">The id of the owner.</param>
        /// <param name="entryId">The id of the entry.</param>
        /// <param name="cancellationToken">Indicates that the start process has been aborted.</param>
        /// <param name="transactionHandle">The database transaction handle.</param>
        /// <returns>A <see cref="Task" /> whose result indicates success.</returns>
        public Task DeleteAsync(
            string userId,
            string entryId,
            CancellationToken cancellationToken,
            ITransactionHandle<TClientSessionHandle> transactionHandle
        )
        {
            if (!this.database.TryGetValue(
                    userId,
                    out var userDictionary) ||
                !userDictionary.Remove(entryId))
            {
                throw new NotFoundException();
            }

            return Task.CompletedTask;
        }

        /// <summary>
        ///     Read all entries from the database provider.
        /// </summary>
        /// <param name="userId">The id of the owner.</param>
        /// <param name="cancellationToken">Indicates that the start process has been aborted.</param>
        /// <param name="transactionHandle">The database transaction handle.</param>
        /// <returns>A <see cref="Task{T}" /> whose result is a list of all entries.</returns>
        public Task<IEnumerable<TEntry>> ReadAsync(
            string userId,
            CancellationToken cancellationToken,
            ITransactionHandle<TClientSessionHandle> transactionHandle
        )
        {
            if (!this.database.TryGetValue(
                    userId,
                    out var userDictionary))
            {
                return Task.FromResult(Enumerable.Empty<TEntry>());
            }

            return Task.FromResult(userDictionary.Values.Select(x => x));
        }

        /// <summary>
        ///     Read an entry by its id.
        /// </summary>
        /// <param name="userId">The id of the owner.</param>
        /// <param name="entryId">The id of the entry.</param>
        /// <param name="cancellationToken">Indicates that the start process has been aborted.</param>
        /// <param name="transactionHandle">The database transaction handle.</param>
        /// <returns>A <see cref="Task{T}" /> whose result is the found entry.</returns>
        public Task<TEntry> ReadByIdAsync(
            string userId,
            string entryId,
            CancellationToken cancellationToken,
            ITransactionHandle<TClientSessionHandle> transactionHandle
        )
        {
            if (this.database.TryGetValue(
                    userId,
                    out var userDictionary) &&
                userDictionary.TryGetValue(
                    entryId,
                    out var entry))
            {
                return Task.FromResult(entry);
            }

            throw new NotFoundException();
        }

        /// <summary>
        ///     Updates the specified entry.
        /// </summary>
        /// <param name="entry">The entry to be replaced.</param>
        /// <param name="cancellationToken">Indicates that the start process has been aborted.</param>
        /// <param name="transactionHandle">The database transaction handle.</param>
        /// <returns>A <see cref="Task" /> whose result indicates success.</returns>
        public Task UpdateAsync(
            TEntry entry,
            CancellationToken cancellationToken,
            ITransactionHandle<TClientSessionHandle> transactionHandle
        )
        {
            if (!this.database.TryGetValue(
                    entry.UserId,
                    out var userDictionary) ||
                !userDictionary.ContainsKey(entry.Id))
            {
                throw new NotFoundException();
            }

            this.database[entry.UserId][entry.Id] = entry;
            return Task.CompletedTask;
        }
    }
}
