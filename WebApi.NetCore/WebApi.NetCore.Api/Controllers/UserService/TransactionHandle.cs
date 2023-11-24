﻿namespace WebApi.NetCore.Api.Controllers.UserService
{
    using Generic.Base.Api.Database;

    public class TransactionHandle : ITransactionHandle<object>
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
        public object ClientSessionHandle { get; }

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