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
    internal class InMemoryProvider<TEntry, TClientSessionHandle> : IProvider<TEntry, TClientSessionHandle>
        where TEntry : IIdEntry
    {
        /// <summary>
        ///     The in-memory database.
        /// </summary>
        private readonly IDictionary<string, TEntry> database = new Dictionary<string, TEntry>();

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
            if (this.database.ContainsKey(entry.Id))
            {
                throw new ConflictException();
            }

            this.database[entry.Id] = entry;
            return Task.FromResult(entry);
        }

        /// <summary>
        ///     Delete an entry by its id.
        /// </summary>
        /// <param name="id">The id of the entry.</param>
        /// <param name="cancellationToken">Indicates that the start process has been aborted.</param>
        /// <param name="transactionHandle">The database transaction handle.</param>
        /// <returns>A <see cref="Task" /> whose result indicates success.</returns>
        public Task DeleteAsync(
            string id,
            CancellationToken cancellationToken,
            ITransactionHandle<TClientSessionHandle> transactionHandle
        )
        {
            if (this.database.ContainsKey(id))
            {
                this.database.Remove(id);
            }

            return Task.CompletedTask;
        }

        /// <summary>
        ///     Read all entries from the database provider.
        /// </summary>
        /// <param name="cancellationToken">Indicates that the start process has been aborted.</param>
        /// <param name="transactionHandle">The database transaction handle.</param>
        /// <returns>A <see cref="Task{T}" /> whose result is a list of all entries.</returns>
        public Task<IEnumerable<TEntry>> ReadAsync(
            CancellationToken cancellationToken,
            ITransactionHandle<TClientSessionHandle> transactionHandle
        )
        {
            return Task.FromResult(this.database.Values.Select(x => x));
        }

        /// <summary>
        ///     Read an entry by its id.
        /// </summary>
        /// <param name="id">The id of the entry.</param>
        /// <param name="cancellationToken">Indicates that the start process has been aborted.</param>
        /// <param name="transactionHandle">The database transaction handle.</param>
        /// <returns>A <see cref="Task{T}" /> whose result is the found entry.</returns>
        public Task<TEntry> ReadByIdAsync(
            string id,
            CancellationToken cancellationToken,
            ITransactionHandle<TClientSessionHandle> transactionHandle
        )
        {
            if (this.database.TryGetValue(
                    id,
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
            if (this.database.ContainsKey(entry.Id))
            {
                this.database[entry.Id] = entry;
                return Task.CompletedTask;
            }

            throw new NotFoundException();
        }
    }
}
