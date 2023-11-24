namespace WebApi.NetCore.Api.Controllers
{
    using Generic.Base.Api.AuthServices;
    using Generic.Base.Api.Database;
    using Generic.Base.Api.Exceptions;

    public class AuthProvider : IProvider<IClaimsUser, object>
    {
        private readonly IDictionary<string, IClaimsUser> database = new Dictionary<string, IClaimsUser>();

        /// <summary>
        ///     Creates the specified entry.
        /// </summary>
        /// <param name="claimsUser">The entry to be created.</param>
        /// <param name="cancellationToken">Indicates that the start process has been aborted.</param>
        /// <param name="transactionHandle">Handle for database transactions.</param>
        /// <returns>A <see cref="Task{T}" /> whose result is the created entry.</returns>
        public Task<IClaimsUser> CreateAsync(
            IClaimsUser claimsUser,
            CancellationToken cancellationToken,
            ITransactionHandle<object> transactionHandle
        )
        {
            if (this.database.ContainsKey(claimsUser.Id))
            {
                throw new ConflictException();
            }

            this.database[claimsUser.Id] = claimsUser;

            return Task.FromResult(claimsUser);
        }

        /// <summary>
        ///     Delete an entry by its id.
        /// </summary>
        /// <param name="id">The id of the entry.</param>
        /// <param name="cancellationToken">Indicates that the start process has been aborted.</param>
        /// <param name="transactionHandle">The database transaction handle.</param>
        /// <returns>A <see cref="Task" /> whose result indicates success.</returns>
        public Task DeleteAsync(
            string id,
            CancellationToken cancellationToken,
            ITransactionHandle<object> transactionHandle
        )
        {
            if (!this.database.ContainsKey(id))
            {
                throw new NotFoundException();
            }

            this.database.Remove(id);

            return Task.CompletedTask;
        }

        /// <summary>
        ///     Read all entries from the database provider.
        /// </summary>
        /// <param name="cancellationToken">Indicates that the start process has been aborted.</param>
        /// <param name="transactionHandle">The database transaction handle.</param>
        /// <returns>A <see cref="Task{T}" /> whose result is a list of all entries.</returns>
        public Task<IEnumerable<IClaimsUser>> ReadAsync(
            CancellationToken cancellationToken,
            ITransactionHandle<object> transactionHandle
        )
        {
            return Task.FromResult(this.database.Values.Select(x => x));
        }

        /// <summary>
        ///     Read an entry by its id.
        /// </summary>
        /// <param name="id">The id of the entry.</param>
        /// <param name="cancellationToken">Indicates that the start process has been aborted.</param>
        /// <param name="transactionHandle">The database transaction handle.</param>
        /// <returns>A <see cref="Task{T}" /> whose result is the found entry.</returns>
        public Task<IClaimsUser> ReadByIdAsync(
            string id,
            CancellationToken cancellationToken,
            ITransactionHandle<object> transactionHandle
        )
        {
            if (this.database.TryGetValue(
                    id,
                    out var claimsUser))
            {
                return Task.FromResult(claimsUser);
            }

            throw new NotFoundException();
        }

        /// <summary>
        ///     Updates the specified entry.
        /// </summary>
        /// <param name="entry">The entry to be replaced.</param>
        /// <param name="cancellationToken">Indicates that the start process has been aborted.</param>
        /// <param name="transactionHandle">The database transaction handle.</param>
        /// <returns>A <see cref="Task" /> whose result indicates success.</returns>
        public Task UpdateAsync(
            IClaimsUser entry,
            CancellationToken cancellationToken,
            ITransactionHandle<object> transactionHandle
        )
        {
            if (this.database.ContainsKey(entry.Id))
            {
                this.database[entry.Id] = entry;
                return Task.CompletedTask;
            }

            throw new NotFoundException();
        }
    }
}
