namespace Generic.Base.Api.Services
{
    using Generic.Base.Api.Database;
    using Generic.Base.Api.Transformer;

    /// <inheritdoc cref="IAtomicService{TCreate,TEntry,TUpdate,TClientSessionHandle}" />
    public class AtomicService<TCreate, TEntry, TUpdate, TClientSessionHandle>
        : IAtomicService<TCreate, TEntry, TUpdate, TClientSessionHandle>
    {
        /// <summary>
        ///     The provider for instances of <typeparamref name="TEntry" />.
        /// </summary>
        private readonly IProvider<TEntry, TClientSessionHandle> provider;

        /// <summary>
        ///     The transformer of entries.
        /// </summary>
        private readonly IAtomicTransformer<TCreate, TEntry, TUpdate> transformer;

        /// <summary>
        ///     Initializes a new instance of the <see cref="AtomicService{TCreate, TEntry, TUpdate, TClientSessionHandle}" />
        ///     class.
        /// </summary>
        /// <param name="provider">The provider of <typeparamref name="TEntry" /> instances.</param>
        /// <param name="transformer">Transforms instances of <typeparamref name="TCreate" /> to <typeparamref name="TEntry" />..</param>
        public AtomicService(
            IProvider<TEntry, TClientSessionHandle> provider,
            IAtomicTransformer<TCreate, TEntry, TUpdate> transformer
        )
        {
            this.provider = provider;
            this.transformer = transformer;
        }

        /// <summary>
        ///     Creates a new entry.
        /// </summary>
        /// <param name="createEntry">The data for creating a new entry.</param>
        /// <param name="cancellationToken">Indicates that the start process has been aborted.</param>
        /// <param name="transactionHandle">The database transaction handle.</param>
        /// <returns>A <see cref="Task{T}" /> whose result is the created entry.</returns>
        public Task<TEntry> CreateAsync(
            TCreate createEntry,
            CancellationToken cancellationToken,
            ITransactionHandle<TClientSessionHandle> transactionHandle
        )
        {
            var entry = this.transformer.Transform(createEntry);
            return this.provider.CreateAsync(
                entry,
                cancellationToken,
                transactionHandle);
        }

        /// <summary>
        ///     Deletes the specified entry by its id.
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
        /// <returns>A <see cref="Task{T}" /> whose result are the found entries.</returns>
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
            return this.provider.ReadByIdAsync(
                id,
                cancellationToken,
                transactionHandle);
        }

        /// <summary>
        ///     Updates an entry by its id.
        /// </summary>
        /// <param name="entry">The entry to be updated.</param>
        /// <param name="id">The id of the entry that is updated.</param>
        /// <param name="cancellationToken">Indicates that the start process has been aborted.</param>
        /// <param name="transactionHandle">The database transaction handle.</param>
        /// <returns>A <see cref="Task" /> whose result indicates success.</returns>
        public Task UpdateAsync(
            TUpdate entry,
            string id,
            CancellationToken cancellationToken,
            ITransactionHandle<TClientSessionHandle> transactionHandle
        )
        {
            return this.provider.UpdateAsync(
                this.transformer.Transform(
                    entry,
                    id),
                cancellationToken,
                transactionHandle);
        }
    }
}
