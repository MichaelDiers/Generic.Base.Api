namespace Generic.Base.Api.Services
{
    using Generic.Base.Api.Database;

    /// <inheritdoc cref="IUserBoundDomainService{TCreate,TEntry,TUpdate}" />
    public class UserBoundDomainService<TCreate, TEntry, TUpdate, TClientSessionHandle>
        : IUserBoundDomainService<TCreate, TEntry, TUpdate>
    {
        /// <summary>
        ///     The atomic service.
        /// </summary>
        private readonly IUserBoundAtomicService<TCreate, TEntry, TUpdate, TClientSessionHandle> atomicService;

        /// <summary>
        ///     The database transaction handler.
        /// </summary>
        private readonly ITransactionHandler<TClientSessionHandle> transactionHandler;

        /// <summary>
        ///     Initializes a new instance of the
        ///     <see cref="UserBoundDomainService{TCreate, TEntry, TUpdate, TClientSessionHandle}" />
        ///     class.
        /// </summary>
        /// <param name="transactionHandler">The database transaction handler.</param>
        /// <param name="atomicService">The atomic service.</param>
        public UserBoundDomainService(
            ITransactionHandler<TClientSessionHandle> transactionHandler,
            IUserBoundAtomicService<TCreate, TEntry, TUpdate, TClientSessionHandle> atomicService
        )
        {
            this.transactionHandler = transactionHandler;
            this.atomicService = atomicService;
        }

        /// <summary>
        ///     Creates a new entry.
        /// </summary>
        /// <param name="userId">The id of the owner.</param>
        /// <param name="createEntry">The data for creating a new entry.</param>
        /// <param name="cancellationToken">Indicates that the start process has been aborted.</param>
        /// <returns>A <see cref="Task{T}" /> whose result is the created entry.</returns>
        public async Task<TEntry> CreateAsync(string userId, TCreate createEntry, CancellationToken cancellationToken)
        {
            using var session = await this.transactionHandler.StartTransactionAsync(cancellationToken);
            try
            {
                var result = await this.atomicService.CreateAsync(
                    userId,
                    createEntry,
                    cancellationToken,
                    session);
                await session.CommitTransactionAsync(cancellationToken);
                return result;
            }
            catch
            {
                await session.AbortTransactionAsync(cancellationToken);
                throw;
            }
        }

        /// <summary>
        ///     Deletes the specified entry by its id.
        /// </summary>
        /// <param name="userId">The id of the owner.</param>
        /// <param name="entryId">The identifier of the entry.</param>
        /// <param name="cancellationToken">Indicates that the start process has been aborted.</param>
        /// <returns>A <see cref="Task" /> whose result indicates success.</returns>
        public async Task DeleteAsync(string userId, string entryId, CancellationToken cancellationToken)
        {
            using var session = await this.transactionHandler.StartTransactionAsync(cancellationToken);
            try
            {
                await this.atomicService.DeleteAsync(
                    userId,
                    entryId,
                    cancellationToken,
                    session);
                await session.CommitTransactionAsync(cancellationToken);
            }
            catch
            {
                await session.AbortTransactionAsync(cancellationToken);
                throw;
            }
        }

        /// <summary>
        ///     Reads all entries.
        /// </summary>
        /// <param name="userId">The id of the owner.</param>
        /// <param name="cancellationToken">Indicates that the start process has been aborted.</param>
        /// <returns>A <see cref="Task{T}" /> whose result are the found entries.</returns>
        public async Task<IEnumerable<TEntry>> ReadAsync(string userId, CancellationToken cancellationToken)
        {
            using var session = await this.transactionHandler.StartTransactionAsync(cancellationToken);
            try
            {
                var result = (await this.atomicService.ReadAsync(
                    userId,
                    cancellationToken,
                    session)).ToArray();
                await session.CommitTransactionAsync(cancellationToken);
                return result;
            }
            catch
            {
                await session.AbortTransactionAsync(cancellationToken);
                throw;
            }
        }

        /// <summary>
        ///     Read an entry by its id.
        /// </summary>
        /// <param name="userId">The id of the owner.</param>
        /// <param name="entryId">The id of the entry.</param>
        /// <param name="cancellationToken">Indicates that the start process has been aborted.</param>
        /// <returns>A <see cref="Task{T}" /> whose result is the found entry.</returns>
        public async Task<TEntry> ReadByIdAsync(string userId, string entryId, CancellationToken cancellationToken)
        {
            using var session = await this.transactionHandler.StartTransactionAsync(cancellationToken);
            try
            {
                var result = await this.atomicService.ReadByIdAsync(
                    userId,
                    entryId,
                    cancellationToken,
                    session);
                await session.CommitTransactionAsync(cancellationToken);
                return result;
            }
            catch
            {
                await session.AbortTransactionAsync(cancellationToken);
                throw;
            }
        }

        /// <summary>
        ///     Updates an entry by its id.
        /// </summary>
        /// <param name="entry">The entry to be updated.</param>
        /// <param name="userId">The id of the owner.</param>
        /// <param name="entryId">The id of the entry that is updated.</param>
        /// <param name="cancellationToken">Indicates that the start process has been aborted.</param>
        /// <returns>A <see cref="Task" /> whose result indicates success.</returns>
        public async Task UpdateAsync(
            TUpdate entry,
            string userId,
            string entryId,
            CancellationToken cancellationToken
        )
        {
            using var session = await this.transactionHandler.StartTransactionAsync(cancellationToken);
            try
            {
                await this.atomicService.UpdateAsync(
                    entry,
                    userId,
                    entryId,
                    cancellationToken,
                    session);
                await session.CommitTransactionAsync(cancellationToken);
            }
            catch
            {
                await session.AbortTransactionAsync(cancellationToken);
                throw;
            }
        }
    }
}
