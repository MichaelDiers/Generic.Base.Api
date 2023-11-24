namespace WebApi.NetCore.Api.Database
{
    using Generic.Base.Api.Database;
    using MongoDB.Driver;

    /// <summary>
    ///     A handler for database transactions.
    /// </summary>
    public class TransactionHandler : ITransactionHandler<IClientSessionHandle>
    {
        /// <summary>
        ///     The mongo client.
        /// </summary>
        private readonly IMongoClient mongoClient;

        /// <summary>
        ///     Initializes a new instance of the <see cref="TransactionHandler" /> class.
        /// </summary>
        /// <param name="mongoClient">The mongo client.</param>
        public TransactionHandler(IMongoClient mongoClient)
        {
            this.mongoClient = mongoClient;
        }

        /// <summary>
        ///     Starts a new transaction.
        /// </summary>
        /// <param name="cancellationToken">Indicates that the start process has been aborted.</param>
        /// <returns>A <see cref="Task" /> whose result is a <see cref="ITransactionHandle{T}" />.</returns>
        public async Task<ITransactionHandle<IClientSessionHandle>> StartTransactionAsync(
            CancellationToken cancellationToken
        )
        {
            var clientSessionHandle = await this.mongoClient.StartSessionAsync(cancellationToken: cancellationToken);
            clientSessionHandle.StartTransaction();
            return new TransactionHandle(clientSessionHandle);
        }
    }
}
