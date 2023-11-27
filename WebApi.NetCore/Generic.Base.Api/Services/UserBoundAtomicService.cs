namespace Generic.Base.Api.Services
{
    using Generic.Base.Api.Database;
    using Generic.Base.Api.Transformer;

    /// <inheritdoc cref="IAtomicService{TCreate,TEntry,TUpdate,TClientSessionHandle}" />
    public class UserBoundAtomicService<TCreate, TEntry, TUpdate, TClientSessionHandle>
        : IUserBoundAtomicService<TCreate, TEntry, TUpdate, TClientSessionHandle>
    {
        /// <summary>
        ///     The provider for instances of <typeparamref name="TEntry" />.
        /// </summary>
        private readonly IUserBoundProvider<TEntry, TClientSessionHandle> provider;

        /// <summary>
        ///     The transformer of entries.
        /// </summary>
        private readonly IUserBoundAtomicTransformer<TCreate, TEntry, TUpdate> transformer;

        /// <summary>
        ///     Initializes a new instance of the <see cref="AtomicService{TCreate, TEntry, TUpdate, TClientSessionHandle}" />
        ///     class.
        /// </summary>
        /// <param name="provider">The provider of <typeparamref name="TEntry" /> instances.</param>
        /// <param name="transformer">Transforms instances of <typeparamref name="TCreate" /> to <typeparamref name="TEntry" />..</param>
        public UserBoundAtomicService(
            IUserBoundProvider<TEntry, TClientSessionHandle> provider,
            IUserBoundAtomicTransformer<TCreate, TEntry, TUpdate> transformer
        )
        {
            this.provider = provider;
            this.transformer = transformer;
        }

        /// <summary>
        ///     Creates a new entry.
        /// </summary>
        /// <param name="userId">The id of he owner.</param>
        /// <param name="createEntry">The data for creating a new entry.</param>
        /// <param name="cancellationToken">Indicates that the start process has been aborted.</param>
        /// <param name="transactionHandle">The database transaction handle.</param>
        /// <returns>A <see cref="Task{T}" /> whose result is the created entry.</returns>
        public Task<TEntry> CreateAsync(
            string userId,
            TCreate createEntry,
            CancellationToken cancellationToken,
            ITransactionHandle<TClientSessionHandle> transactionHandle
        )
        {
            var entry = this.transformer.Transform(
                createEntry,
                userId);
            return this.provider.CreateAsync(
                entry,
                cancellationToken,
                transactionHandle);
        }

        /// <summary>
        ///     Deletes the specified entry by its id.
        /// </summary>
        /// <param name="userId">The identifier of the owner.</param>
        /// <param name="entryId">The identifier of the entry.</param>
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
            return this.provider.DeleteAsync(
                userId,
                entryId,
                cancellationToken,
                transactionHandle);
        }

        /// <summary>
        ///     Reads all entries.
        /// </summary>
        /// <param name="userId">The identifier of the owner.</param>
        /// <param name="cancellationToken">Indicates that the start process has been aborted.</param>
        /// <param name="transactionHandle">The database transaction handle.</param>
        /// <returns>A <see cref="Task{T}" /> whose result are the found entries.</returns>
        public Task<IEnumerable<TEntry>> ReadAsync(
            string userId,
            CancellationToken cancellationToken,
            ITransactionHandle<TClientSessionHandle> transactionHandle
        )
        {
            return this.provider.ReadAsync(
                userId,
                cancellationToken,
                transactionHandle);
        }

        /// <summary>
        ///     Read an entry by its id.
        /// </summary>
        /// <param name="userId">The identifier of the owner.</param>
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
            return this.provider.ReadByIdAsync(
                userId,
                entryId,
                cancellationToken,
                transactionHandle);
        }

        /// <summary>
        ///     Updates an entry by its id.
        /// </summary>
        /// <param name="entry">The entry to be updated.</param>
        /// <param name="userId">The identifier of the owner.</param>
        /// <param name="entryId">The id of the entry that is updated.</param>
        /// <param name="cancellationToken">Indicates that the start process has been aborted.</param>
        /// <param name="transactionHandle">The database transaction handle.</param>
        /// <returns>A <see cref="Task" /> whose result indicates success.</returns>
        public Task UpdateAsync(
            TUpdate entry,
            string userId,
            string entryId,
            CancellationToken cancellationToken,
            ITransactionHandle<TClientSessionHandle> transactionHandle
        )
        {
            return this.provider.UpdateAsync(
                this.transformer.Transform(
                    entry,
                    userId,
                    entryId),
                cancellationToken,
                transactionHandle);
        }
    }
}
