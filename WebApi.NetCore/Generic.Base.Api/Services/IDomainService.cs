namespace Generic.Base.Api.Services
{
    /// <summary>
    ///     Describes the operations of a domain service.
    /// </summary>
    /// <typeparam name="TCreate">The data for creating an instance of <typeparamref name="TEntry" />.</typeparam>
    /// <typeparam name="TEntry">The type of the entry.</typeparam>
    /// <typeparam name="TUpdate">The data for updating an instance of <typeparamref name="TEntry" />.</typeparam>
    public interface IDomainService<in TCreate, TEntry, in TUpdate>
    {
        /// <summary>
        ///     Creates a new entry.
        /// </summary>
        /// <param name="createEntry">The data for creating a new entry.</param>
        /// <param name="cancellationToken">Indicates that the start process has been aborted.</param>
        /// <returns>A <see cref="Task{T}" /> whose result is the created entry.</returns>
        Task<TEntry> CreateAsync(TCreate createEntry, CancellationToken cancellationToken);

        /// <summary>
        ///     Deletes the specified entry by its id.
        /// </summary>
        /// <param name="id">The identifier of the entry.</param>
        /// <param name="cancellationToken">Indicates that the start process has been aborted.</param>
        /// <returns>A <see cref="Task" /> whose result indicates success.</returns>
        Task DeleteAsync(string id, CancellationToken cancellationToken);

        /// <summary>
        ///     Reads all entries.
        /// </summary>
        /// <param name="cancellationToken">Indicates that the start process has been aborted.</param>
        /// <returns>A <see cref="Task{T}" /> whose result are the found entries.</returns>
        Task<IEnumerable<TEntry>> ReadAsync(CancellationToken cancellationToken);

        /// <summary>
        ///     Read an entry by its id.
        /// </summary>
        /// <param name="id">The id of the entry.</param>
        /// <param name="cancellationToken">Indicates that the start process has been aborted.</param>
        /// <returns>A <see cref="Task{T}" /> whose result is the found entry.</returns>
        Task<TEntry> ReadByIdAsync(string id, CancellationToken cancellationToken);

        /// <summary>
        ///     Updates an entry by its id.
        /// </summary>
        /// <param name="entry">The entry to be updated.</param>
        /// <param name="id">The id of the entry that is updated.</param>
        /// <param name="cancellationToken">Indicates that the start process has been aborted.</param>
        /// <returns>A <see cref="Task" /> whose result indicates success.</returns>
        Task UpdateAsync(TUpdate entry, string id, CancellationToken cancellationToken);
    }
}
