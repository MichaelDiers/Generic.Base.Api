namespace WebApi.NetCore.Api.Contracts.Services.Atomic
{
    using WebApi.NetCore.Api.Contracts.Database;

    /// <summary>
    ///     Base description of an atomic service.
    /// </summary>
    /// <typeparam name="TCreate">The data for creating an instance of <typeparamref name="TEntry" />.</typeparam>
    /// <typeparam name="TEntry">The type of the entry.</typeparam>
    /// <typeparam name="TClientSessionHandle">The type of the database transaction handle.</typeparam>
    public interface IAtomicService<in TCreate, TEntry, TClientSessionHandle>
    {
        /// <summary>
        ///     Creates a new entry.
        /// </summary>
        /// <param name="createEntry">The create entry.</param>
        /// <param name="cancellationToken">Indicates that the start process has been aborted.</param>
        /// <param name="transactionHandle">The database transaction handle.</param>
        /// <returns>A <see cref="Task" /> whose result is the created entry.</returns>
        Task<TEntry> CreateAsync(
            TCreate createEntry,
            CancellationToken cancellationToken,
            ITransactionHandle<TClientSessionHandle> transactionHandle
        );

        /// <summary>
        ///     Deletes the entry by its id.
        /// </summary>
        /// <param name="id">The identifier of the entry.</param>
        /// <param name="cancellationToken">Indicates that the start process has been aborted.</param>
        /// <param name="transactionHandle">The database transaction handle.</param>
        /// <returns>A <see cref="Task" /> whose result indicates success.</returns>
        Task DeleteAsync(
            string id,
            CancellationToken cancellationToken,
            ITransactionHandle<TClientSessionHandle> transactionHandle
        );

        /// <summary>
        ///     Reads all entries.
        /// </summary>
        /// <param name="cancellationToken">Indicates that the start process has been aborted.</param>
        /// <param name="transactionHandle">The database transaction handle.</param>
        /// <returns>A <see cref="Task{TResult}" /> whose result are the found entries.</returns>
        Task<IEnumerable<TEntry>> ReadAsync(
            CancellationToken cancellationToken,
            ITransactionHandle<TClientSessionHandle> transactionHandle
        );

        /// <summary>
        ///     Read entry by its id used in the application.
        /// </summary>
        /// <param name="id">The id of the entry.</param>
        /// <param name="cancellationToken">Indicates that the start process has been aborted.</param>
        /// <param name="transactionHandle">The database transaction handle.</param>
        /// <returns>A <see cref="Task" /> whose result is the found entry.</returns>
        Task<TEntry> ReadByIdAsync(
            string id,
            CancellationToken cancellationToken,
            ITransactionHandle<TClientSessionHandle> transactionHandle
        );

        /// <summary>
        ///     Updates the entry by its id.
        /// </summary>
        /// <param name="entry">The entry to be updated.</param>
        /// <param name="cancellationToken">Indicates that the start process has been aborted.</param>
        /// <param name="transactionHandle">The database transaction handle.</param>
        /// <returns>A <see cref="Task" /> whose result indicates success.</returns>
        Task UpdateAsync(
            TEntry entry,
            CancellationToken cancellationToken,
            ITransactionHandle<TClientSessionHandle> transactionHandle
        );
    }
}
