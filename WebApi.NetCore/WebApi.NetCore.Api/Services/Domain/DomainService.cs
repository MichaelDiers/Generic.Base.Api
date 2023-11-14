namespace WebApi.NetCore.Api.Services.Domain
{
    using WebApi.NetCore.Api.Contracts.Database;
    using WebApi.NetCore.Api.Contracts.Services.Atomic;
    using WebApi.NetCore.Api.Contracts.Services.Domain;

    /// <inheritdoc cref="IDomainService{TCreate,TEntry}" />
    public class DomainService<TCreate, TEntry, TClientSessionHandle> : IDomainService<TCreate, TEntry>
    {
        /// <summary>
        ///     The atomic service.
        /// </summary>
        private readonly IAtomicService<TCreate, TEntry, TClientSessionHandle> atomicService;

        /// <summary>
        ///     The database transaction handler.
        /// </summary>
        private readonly ITransactionHandler<TClientSessionHandle> transactionHandler;

        /// <summary>
        ///     Initializes a new instance of the <see cref="DomainService{TCreate, TEntry, TClientSessionHandle}" /> class.
        /// </summary>
        /// <param name="transactionHandler">The database transaction handler.</param>
        /// <param name="atomicService">The atomic service.</param>
        public DomainService(
            ITransactionHandler<TClientSessionHandle> transactionHandler,
            IAtomicService<TCreate, TEntry, TClientSessionHandle> atomicService
        )

        {
            this.transactionHandler = transactionHandler;
            this.atomicService = atomicService;
        }

        /// <summary>
        ///     Creates a new entry.
        /// </summary>
        /// <param name="createEntry">The create entry.</param>
        /// <param name="cancellationToken">Indicates that the start process has been aborted.</param>
        /// <returns>A <see cref="Task" /> whose result is the created entry.</returns>
        public async Task<TEntry> CreateAsync(TCreate createEntry, CancellationToken cancellationToken)
        {
            using var session = await this.transactionHandler.StartTransactionAsync(cancellationToken);
            try
            {
                var result = await this.atomicService.CreateAsync(
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
        ///     Deletes the entry by its id.
        /// </summary>
        /// <param name="id">The identifier of the entry.</param>
        /// <param name="cancellationToken">Indicates that the start process has been aborted.</param>
        /// <returns>A <see cref="Task" /> whose result indicates success.</returns>
        public async Task DeleteAsync(string id, CancellationToken cancellationToken)
        {
            using var session = await this.transactionHandler.StartTransactionAsync(cancellationToken);
            try
            {
                await this.atomicService.DeleteAsync(
                    id,
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
        /// <param name="cancellationToken">Indicates that the start process has been aborted.</param>
        /// <returns>A <see cref="Task{TResult}" /> whose result are the found entries.</returns>
        public async Task<IEnumerable<TEntry>> ReadAsync(CancellationToken cancellationToken)
        {
            using var session = await this.transactionHandler.StartTransactionAsync(cancellationToken);
            try
            {
                var result = (await this.atomicService.ReadAsync(
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
        ///     Read entry by its id used in the application.
        /// </summary>
        /// <param name="id">The id of the entry.</param>
        /// <param name="cancellationToken">Indicates that the start process has been aborted.</param>
        /// <returns>A <see cref="Task" /> whose result is the found entry.</returns>
        public async Task<TEntry> ReadByIdAsync(string id, CancellationToken cancellationToken)
        {
            using var session = await this.transactionHandler.StartTransactionAsync(cancellationToken);
            try
            {
                var result = await this.atomicService.ReadByIdAsync(
                    id,
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
        ///     Updates the entry by its id.
        /// </summary>
        /// <param name="entry">The entry to be updated.</param>
        /// <param name="cancellationToken">Indicates that the start process has been aborted.</param>
        /// <returns>A <see cref="Task" /> whose result indicates success.</returns>
        public async Task UpdateAsync(TEntry entry, CancellationToken cancellationToken)
        {
            using var session = await this.transactionHandler.StartTransactionAsync(cancellationToken);
            try
            {
                await this.atomicService.UpdateAsync(
                    entry,
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
