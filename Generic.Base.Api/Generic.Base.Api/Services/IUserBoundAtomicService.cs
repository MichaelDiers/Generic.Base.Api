namespace Generic.Base.Api.Services
{
    using Generic.Base.Api.Database;

    /// <summary>
    ///     Describes the operations of an atomic service.
    /// </summary>
    /// <typeparam name="TCreate">The data for creating an instance of <typeparamref name="TEntry" />.</typeparam>
    /// <typeparam name="TEntry">The type of the entry.</typeparam>
    /// <typeparam name="TUpdate">The data for updating an instance of <typeparamref name="TEntry" />.</typeparam>
    /// <typeparam name="TClientSessionHandle">The type of the database transaction handle.</typeparam>
    public interface IUserBoundAtomicService<in TCreate, TEntry, in TUpdate, in TClientSessionHandle>
    {
        /// <summary>
        ///     Creates a new entry.
        /// </summary>
        /// <param name="userId">The id of the owner.</param>
        /// <param name="createEntry">The data for creating a new entry.</param>
        /// <param name="cancellationToken">Indicates that the start process has been aborted.</param>
        /// <param name="transactionHandle">The database transaction handle.</param>
        /// <returns>A <see cref="Task{T}" /> whose result is the created entry.</returns>
        Task<TEntry> CreateAsync(
            string userId,
            TCreate createEntry,
            CancellationToken cancellationToken,
            ITransactionHandle<TClientSessionHandle> transactionHandle
        );

        /// <summary>
        ///     Deletes the specified entry by its id.
        /// </summary>
        /// <param name="userId">The identifier of the owner.</param>
        /// <param name="entryId">The identifier of the entry.</param>
        /// <param name="cancellationToken">Indicates that the start process has been aborted.</param>
        /// <param name="transactionHandle">The database transaction handle.</param>
        /// <returns>A <see cref="Task" /> whose result indicates success.</returns>
        Task DeleteAsync(
            string userId,
            string entryId,
            CancellationToken cancellationToken,
            ITransactionHandle<TClientSessionHandle> transactionHandle
        );

        /// <summary>
        ///     Reads all entries.
        /// </summary>
        /// <param name="userId">The identifier of the owner.</param>
        /// <param name="cancellationToken">Indicates that the start process has been aborted.</param>
        /// <param name="transactionHandle">The database transaction handle.</param>
        /// <returns>A <see cref="Task{T}" /> whose result are the found entries.</returns>
        Task<IEnumerable<TEntry>> ReadAsync(
            string userId,
            CancellationToken cancellationToken,
            ITransactionHandle<TClientSessionHandle> transactionHandle
        );

        /// <summary>
        ///     Read an entry by its id.
        /// </summary>
        /// <param name="userId">The identifier of the owner.</param>
        /// <param name="entryId">The id of the entry.</param>
        /// <param name="cancellationToken">Indicates that the start process has been aborted.</param>
        /// <param name="transactionHandle">The database transaction handle.</param>
        /// <returns>A <see cref="Task{T}" /> whose result is the found entry.</returns>
        Task<TEntry> ReadByIdAsync(
            string userId,
            string entryId,
            CancellationToken cancellationToken,
            ITransactionHandle<TClientSessionHandle> transactionHandle
        );

        /// <summary>
        ///     Updates an entry by its id.
        /// </summary>
        /// <param name="entry">The entry to be updated.</param>
        /// <param name="userId">The identifier of the owner.</param>
        /// <param name="entryId">The id of the entry that is updated.</param>
        /// <param name="cancellationToken">Indicates that the start process has been aborted.</param>
        /// <param name="transactionHandle">The database transaction handle.</param>
        /// <returns>A <see cref="Task" /> whose result indicates success.</returns>
        Task UpdateAsync(
            TUpdate entry,
            string userId,
            string entryId,
            CancellationToken cancellationToken,
            ITransactionHandle<TClientSessionHandle> transactionHandle
        );
    }
}
