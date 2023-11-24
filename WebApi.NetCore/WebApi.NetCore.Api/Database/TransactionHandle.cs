namespace WebApi.NetCore.Api.Database
{
    using Generic.Base.Api.Database;
    using MongoDB.Driver;

    /// <summary>
    ///     A database transaction handle.
    /// </summary>
    public class TransactionHandle : ITransactionHandle<IClientSessionHandle>
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="TransactionHandle" /> class.
        /// </summary>
        /// <param name="clientSessionHandle">The client session handle.</param>
        public TransactionHandle(IClientSessionHandle clientSessionHandle)
        {
            this.ClientSessionHandle = clientSessionHandle;
        }

        /// <summary>
        ///     Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            this.ClientSessionHandle.Dispose();
        }

        /// <summary>
        ///     Aborts the transaction.
        /// </summary>
        /// <param name="cancellationToken">Indicates that the start process has been aborted.</param>
        /// <returns>A <see cref="Task" /> whose result indicates success.</returns>
        public async Task AbortTransactionAsync(CancellationToken cancellationToken)
        {
            await this.ClientSessionHandle.AbortTransactionAsync(cancellationToken);
        }

        /// <summary>
        ///     Gets the client session handle.
        /// </summary>
        public IClientSessionHandle ClientSessionHandle { get; }

        /// <summary>
        ///     Commits the transaction.
        /// </summary>
        /// <param name="cancellationToken">Indicates that the start process has been aborted.</param>
        /// <returns>A <see cref="Task" /> whose result indicates success.</returns>
        public async Task CommitTransactionAsync(CancellationToken cancellationToken)
        {
            await this.ClientSessionHandle.CommitTransactionAsync(cancellationToken);
        }
    }
}
