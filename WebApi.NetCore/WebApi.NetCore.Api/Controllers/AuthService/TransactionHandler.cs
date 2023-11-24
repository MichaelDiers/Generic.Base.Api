namespace WebApi.NetCore.Api.Controllers.AuthService
{
    using Generic.Base.Api.Database;

    public class TransactionHandler : ITransactionHandler<object>
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
