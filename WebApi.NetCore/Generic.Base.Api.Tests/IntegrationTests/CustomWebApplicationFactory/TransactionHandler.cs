namespace Generic.Base.Api.Tests.IntegrationTests.CustomWebApplicationFactory
{
    using Generic.Base.Api.Database;

    /// <summary>
    ///     A dummy implementation of a database transaction handler.
    /// </summary>
    /// <seealso cref="Generic.Base.Api.Database.ITransactionHandler&lt;System.Object&gt;" />
    internal class TransactionHandler : ITransactionHandler<object>
    {
        /// <summary>
        ///     Starts a new transaction.
        /// </summary>
        /// <param name="cancellationToken">Indicates that the start process has been aborted.</param>
        /// <returns>A <see cref="Task{T}" /> whose result is a <see cref="ITransactionHandle{T}" />.</returns>
        public Task<ITransactionHandle<object>> StartTransactionAsync(CancellationToken cancellationToken)
        {
            return Task.FromResult<ITransactionHandle<object>>(new TransactionHandle());
        }
    }
}
