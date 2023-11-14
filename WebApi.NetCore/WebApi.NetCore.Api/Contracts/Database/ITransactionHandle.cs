namespace WebApi.NetCore.Api.Contracts.Database
{
    /// <summary>
    ///     A database transaction handle.
    /// </summary>
    /// <typeparam name="TClientSessionHandle">The type of the database transaction handle.</typeparam>
    /// <seealso cref="System.IDisposable" />
    public interface ITransactionHandle<out TClientSessionHandle> : IDisposable
    {
        /// <summary>
        ///     Gets the client session handle.
        /// </summary>
        TClientSessionHandle ClientSessionHandle { get; }

        /// <summary>
        ///     Aborts the transaction.
        /// </summary>
        /// <param name="cancellationToken">Indicates that the start process has been aborted.</param>
        /// <returns>A <see cref="Task" /> whose result indicates success.</returns>
        Task AbortTransactionAsync(CancellationToken cancellationToken);

        /// <summary>
        ///     Commits the transaction.
        /// </summary>
        /// <param name="cancellationToken">Indicates that the start process has been aborted.</param>
        /// <returns>A <see cref="Task" /> whose result indicates success.</returns>
        Task CommitTransactionAsync(CancellationToken cancellationToken);
    }
}
