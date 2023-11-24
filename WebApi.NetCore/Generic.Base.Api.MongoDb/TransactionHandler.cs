namespace Generic.Base.Api.MongoDb.AuthServices
{
    using Generic.Base.Api.Database;
    using MongoDB.Driver;

    /// <summary>
    ///     An implementation of a database transaction handler.
    /// </summary>
    internal class TransactionHandler : ITransactionHandler<IClientSessionHandle>
    {
        /// <summary>
        ///     The mongo db client.
        /// </summary>
        private readonly IMongoClient client;

        /// <summary>
        ///     Initializes a new instance of the <see cref="TransactionHandler" /> class.
        /// </summary>
        /// <param name="client">The mongo db client.</param>
        public TransactionHandler(IMongoClient client)
        {
            this.client = client;
        }

        /// <summary>
        ///     Starts a new transaction.
        /// </summary>
        /// <param name="cancellationToken">Indicates that the start process has been aborted.</param>
        /// <returns>A <see cref="Task{T}" /> whose result is a <see cref="ITransactionHandle{T}" />.</returns>
        public async Task<ITransactionHandle<IClientSessionHandle>> StartTransactionAsync(
            CancellationToken cancellationToken
        )
        {
            var session = await this.client.StartSessionAsync(cancellationToken: cancellationToken);
            session.StartTransaction();
            return new TransactionHandle(session);
        }
    }
}
