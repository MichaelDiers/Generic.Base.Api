namespace Generic.Base.Api.MongoDb
{
    using Generic.Base.Api.Database;
    using MongoDB.Driver;

    /// <summary>
    ///     An implementation of a database transaction handle.
    /// </summary>
    internal class TransactionHandle : ITransactionHandle<IClientSessionHandle>
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="TransactionHandle" /> class.
        /// </summary>
        /// <param name="session">The mongo db session.</param>
        public TransactionHandle(IClientSessionHandle session)
        {
            this.ClientSessionHandle = session;
        }

        /// <summary>Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.</summary>
        public void Dispose()
        {
            this.ClientSessionHandle.Dispose();
        }

        /// <summary>
        ///     Aborts the database transaction.
        /// </summary>
        /// <param name="cancellationToken">Indicates that the start process has been aborted.</param>
        /// <returns>A <see cref="Task" /> whose result indicates success.</returns>
        public async Task AbortTransactionAsync(CancellationToken cancellationToken)
        {
            await this.ClientSessionHandle.AbortTransactionAsync(cancellationToken);
        }

        /// <summary>
        ///     Gets the client session handle for database transactions.
        /// </summary>
        public IClientSessionHandle ClientSessionHandle { get; }

        /// <summary>
        ///     Commits the database transaction.
        /// </summary>
        /// <param name="cancellationToken">Indicates that the start process has been aborted.</param>
        /// <returns>A <see cref="Task" /> whose result indicates success.</returns>
        public async Task CommitTransactionAsync(CancellationToken cancellationToken)
        {
            await this.ClientSessionHandle.CommitTransactionAsync(cancellationToken);
        }
    }
}
