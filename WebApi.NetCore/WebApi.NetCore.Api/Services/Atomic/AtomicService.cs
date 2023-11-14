namespace WebApi.NetCore.Api.Services.Atomic
{
    using WebApi.NetCore.Api.Contracts.Database;
    using WebApi.NetCore.Api.Contracts.Services.Atomic;

    /// <inheritdoc cref="IAtomicService{TCreate,TEntry,TClientSessionHandle}" />
    public class AtomicService<TCreate, TEntry, TClientSessionHandle>
        : IAtomicService<TCreate, TEntry, TClientSessionHandle>
    {
        /// <summary>
        ///     The provider for instances of <typeparamref name="TEntry" />.
        /// </summary>
        private readonly IProvider<TEntry, TClientSessionHandle> provider;

        /// <summary>
        ///     Transforms instances of <typeparamref name="TCreate" /> to <typeparamref name="TEntry" />.
        /// </summary>
        private readonly Func<TCreate, TEntry> toEntry;

        /// <summary>
        ///     Initializes a new instance of the <see cref="AtomicService{TCreate, TEntry, TClientSessionHandle}" /> class.
        /// </summary>
        /// <param name="provider">The provider of <typeparamref name="TEntry" /> instances.</param>
        /// <param name="toEntry">Transforms instances of <typeparamref name="TCreate" /> to <typeparamref name="TEntry" />..</param>
        public AtomicService(IProvider<TEntry, TClientSessionHandle> provider, Func<TCreate, TEntry> toEntry)
        {
            this.provider = provider;
            this.toEntry = toEntry;
        }

        /// <summary>
        ///     Creates a new entry.
        /// </summary>
        /// <param name="createEntry">The create entry.</param>
        /// <param name="cancellationToken">Indicates that the start process has been aborted.</param>
        /// <param name="transactionHandle">The database transaction handle.</param>
        /// <returns>A <see cref="Task" /> whose result is the created entry.</returns>
        public async Task<TEntry> CreateAsync(
            TCreate createEntry,
            CancellationToken cancellationToken,
            ITransactionHandle<TClientSessionHandle> transactionHandle
        )
        {
            var entry = this.toEntry(createEntry);
            await this.provider.CreateAsync(
                entry,
                cancellationToken,
                transactionHandle);
            return entry;
        }

        /// <summary>
        ///     Deletes the entry by its id.
        /// </summary>
        /// <param name="id">The identifier of the entry.</param>
        /// <param name="cancellationToken">Indicates that the start process has been aborted.</param>
        /// <param name="transactionHandle">The database transaction handle.</param>
        /// <returns>A <see cref="Task" /> whose result indicates success.</returns>
        public Task DeleteAsync(
            string id,
            CancellationToken cancellationToken,
            ITransactionHandle<TClientSessionHandle> transactionHandle
        )
        {
            return this.provider.DeleteAsync(
                id,
                cancellationToken,
                transactionHandle);
        }

        /// <summary>
        ///     Reads all entries.
        /// </summary>
        /// <param name="cancellationToken">Indicates that the start process has been aborted.</param>
        /// <param name="transactionHandle">The database transaction handle.</param>
        /// <returns>A <see cref="Task{TResult}" /> whose result are the found entries.</returns>
        public Task<IEnumerable<TEntry>> ReadAsync(
            CancellationToken cancellationToken,
            ITransactionHandle<TClientSessionHandle> transactionHandle
        )
        {
            return this.provider.ReadAsync(
                cancellationToken,
                transactionHandle);
        }

        /// <summary>
        ///     Read entry by its id used in the application.
        /// </summary>
        /// <param name="id">The id of the entry.</param>
        /// <param name="cancellationToken">Indicates that the start process has been aborted.</param>
        /// <param name="transactionHandle">The database transaction handle.</param>
        /// <returns>A <see cref="Task" /> whose result is the found entry.</returns>
        public Task<TEntry> ReadByIdAsync(
            string id,
            CancellationToken cancellationToken,
            ITransactionHandle<TClientSessionHandle> transactionHandle
        )
        {
            return this.provider.ReadByIdAsync(
                id,
                cancellationToken,
                transactionHandle);
        }

        /// <summary>
        ///     Updates the entry by its id.
        /// </summary>
        /// <param name="entry">The entry to be updated.</param>
        /// <param name="cancellationToken">Indicates that the start process has been aborted.</param>
        /// <param name="transactionHandle">The database transaction handle.</param>
        /// <returns>A <see cref="Task" /> whose result indicates success.</returns>
        public Task UpdateAsync(
            TEntry entry,
            CancellationToken cancellationToken,
            ITransactionHandle<TClientSessionHandle> transactionHandle
        )
        {
            return this.provider.UpdateAsync(
                entry,
                cancellationToken,
                transactionHandle);
        }
    }
}
