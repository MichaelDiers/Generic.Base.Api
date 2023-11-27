namespace Generic.Base.Api.Services
{
    /// <summary>
    ///     Describes the operations of a domain service.
    /// </summary>
    /// <typeparam name="TCreate">The data for creating an instance of <typeparamref name="TEntry" />.</typeparam>
    /// <typeparam name="TEntry">The type of the entry.</typeparam>
    /// <typeparam name="TUpdate">The data for updating an instance of <typeparamref name="TEntry" />.</typeparam>
    public interface IUserBoundDomainService<in TCreate, TEntry, in TUpdate>
    {
        /// <summary>
        ///     Creates a new entry.
        /// </summary>
        /// <param name="userId">The id of the owner.</param>
        /// <param name="createEntry">The data for creating a new entry.</param>
        /// <param name="cancellationToken">Indicates that the start process has been aborted.</param>
        /// <returns>A <see cref="Task{T}" /> whose result is the created entry.</returns>
        Task<TEntry> CreateAsync(string userId, TCreate createEntry, CancellationToken cancellationToken);

        /// <summary>
        ///     Deletes the specified entry by its id.
        /// </summary>
        /// <param name="userId">The id of the owner.</param>
        /// <param name="entryId">The identifier of the entry.</param>
        /// <param name="cancellationToken">Indicates that the start process has been aborted.</param>
        /// <returns>A <see cref="Task" /> whose result indicates success.</returns>
        Task DeleteAsync(string userId, string entryId, CancellationToken cancellationToken);

        /// <summary>
        ///     Reads all entries.
        /// </summary>
        /// <param name="userId">The id of the owner.</param>
        /// <param name="cancellationToken">Indicates that the start process has been aborted.</param>
        /// <returns>A <see cref="Task{T}" /> whose result are the found entries.</returns>
        Task<IEnumerable<TEntry>> ReadAsync(string userId, CancellationToken cancellationToken);

        /// <summary>
        ///     Read an entry by its id.
        /// </summary>
        /// <param name="userId">The id of the owner.</param>
        /// <param name="entryId">The id of the entry.</param>
        /// <param name="cancellationToken">Indicates that the start process has been aborted.</param>
        /// <returns>A <see cref="Task{T}" /> whose result is the found entry.</returns>
        Task<TEntry> ReadByIdAsync(string userId, string entryId, CancellationToken cancellationToken);

        /// <summary>
        ///     Updates an entry by its id.
        /// </summary>
        /// <param name="entry">The entry to be updated.</param>
        /// <param name="userId">The id of the owner.</param>
        /// <param name="entryId">The id of the entry that is updated.</param>
        /// <param name="cancellationToken">Indicates that the start process has been aborted.</param>
        /// <returns>A <see cref="Task" /> whose result indicates success.</returns>
        Task UpdateAsync(
            TUpdate entry,
            string userId,
            string entryId,
            CancellationToken cancellationToken
        );
    }
}
