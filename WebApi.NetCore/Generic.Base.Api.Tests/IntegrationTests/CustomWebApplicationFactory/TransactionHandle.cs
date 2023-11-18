namespace Generic.Base.Api.Tests.IntegrationTests.CustomWebApplicationFactory
{
    using Generic.Base.Api.Database;

    /// <summary>
    ///     A dummy implementation of a database transaction handle.
    /// </summary>
    /// <seealso cref="Generic.Base.Api.Database.ITransactionHandle&lt;System.Object&gt;" />
    internal class TransactionHandle : ITransactionHandle<object>
    {
        /// <summary>Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.</summary>
        public void Dispose()
        {
        }

        /// <summary>
        ///     Aborts the database transaction.
        /// </summary>
        /// <param name="cancellationToken">Indicates that the start process has been aborted.</param>
        /// <returns>A <see cref="Task" /> whose result indicates success.</returns>
        public Task AbortTransactionAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        /// <summary>
        ///     Gets the client session handle for database transactions.
        /// </summary>
        public object ClientSessionHandle => new();

        /// <summary>
        ///     Commits the database transaction.
        /// </summary>
        /// <param name="cancellationToken">Indicates that the start process has been aborted.</param>
        /// <returns>A <see cref="Task" /> whose result indicates success.</returns>
        public Task CommitTransactionAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
