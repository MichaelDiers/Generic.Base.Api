namespace WebApi.NetCore.Api.Contracts.Services.Domain
{
    /// <summary>
    ///     Base description of an atomic service.
    /// </summary>
    public interface IDomainService<in TCreate, TEntry>
    {
        /// <summary>
        ///     Creates a new entry.
        /// </summary>
        /// <param name="createEntry">The create entry.</param>
        /// <param name="cancellationToken">Indicates that the start process has been aborted.</param>
        /// <returns>A <see cref="Task" /> whose result is the created entry.</returns>
        Task<TEntry> CreateAsync(TCreate createEntry, CancellationToken cancellationToken);

        /// <summary>
        ///     Deletes the entry by its id.
        /// </summary>
        /// <param name="id">The identifier of the entry.</param>
        /// <param name="cancellationToken">Indicates that the start process has been aborted.</param>
        /// <returns>A <see cref="Task" /> whose result indicates success.</returns>
        Task DeleteAsync(string id, CancellationToken cancellationToken);

        /// <summary>
        ///     Reads all entries.
        /// </summary>
        /// <param name="cancellationToken">Indicates that the start process has been aborted.</param>
        /// <returns>A <see cref="Task{TResult}" /> whose result are the found entries.</returns>
        Task<IEnumerable<TEntry>> ReadAsync(CancellationToken cancellationToken);

        /// <summary>
        ///     Read entry by its id used in the application.
        /// </summary>
        /// <param name="id">The id of the entry.</param>
        /// <param name="cancellationToken">Indicates that the start process has been aborted.</param>
        /// <returns>A <see cref="Task" /> whose result is the found entry.</returns>
        Task<TEntry> ReadByIdAsync(string id, CancellationToken cancellationToken);

        /// <summary>
        ///     Updates the entry by its id.
        /// </summary>
        /// <param name="entry">The entry to be updated.</param>
        /// <param name="cancellationToken">Indicates that the start process has been aborted.</param>
        /// <returns>A <see cref="Task" /> whose result indicates success.</returns>
        Task UpdateAsync(TEntry entry, CancellationToken cancellationToken);
    }
}
