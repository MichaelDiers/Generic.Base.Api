namespace WebApi.NetCore.Api.Contracts.Database
{
    /// <summary>
    ///     A handler for database transactions.
    /// </summary>
    /// <typeparam name="TClientSessionHandle">The type of the database transaction handle.</typeparam>
    public interface ITransactionHandler<TClientSessionHandle>
    {
        /// <summary>
        ///     Starts a new transaction.
        /// </summary>
        /// <param name="cancellationToken">Indicates that the start process has been aborted.</param>
        /// <returns>A <see cref="Task" /> whose result is a <see cref="ITransactionHandle{T}" />.</returns>
        Task<ITransactionHandle<TClientSessionHandle>> StartTransactionAsync(CancellationToken cancellationToken);
    }
}
