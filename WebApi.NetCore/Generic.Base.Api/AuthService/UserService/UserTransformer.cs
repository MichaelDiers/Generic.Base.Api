namespace Generic.Base.Api.AuthService.UserService
{
    using Generic.Base.Api.Result;
    using Generic.Base.Api.Transformer;

    internal class UserTransformer : IControllerTransformer<User, ResultUser>, IAtomicTransformer<User, User, User>
    {
        /// <summary>
        ///     Transforms the specified <paramref name="createEntry" /> of type <typeparamref name="TCreateEntry" /> to
        ///     <typeparamref name="TEntry" />.
        /// </summary>
        /// <param name="createEntry">The data for creating an entry of type <typeparamref name="TEntry" />.</param>
        /// <returns>The transformed entry of type <typeparamref name="TEntry" />.</returns>
        public User Transform(User user)
        {
            return user;
        }

        /// <summary>
        ///     Transforms the specified <paramref name="updateEntry" /> of type <typeparamref name="TUpdateEntry" /> to
        ///     <typeparamref name="TEntry" />.
        /// </summary>
        /// <param name="updateEntry">The data for creating an entry of type <typeparamref name="TEntry" />.</param>
        /// <param name="id">The id of the entry to be updated.</param>
        /// <returns>The transformed entry of type <typeparamref name="TEntry" />.</returns>
        public User Transform(User user, string id)
        {
            return user;
        }

        /// <summary>
        ///     Transforms the specified entry of <see cref="User" /> to <see cref="ResultUser" />.
        /// </summary>
        /// <param name="entry">The data for creating an instance of <see cref="ResultUser" />.</param>
        /// <param name="links">The urn that describe the supported operations on the result entry.</param>
        /// <returns>The transformed entry of type <see cref="User" />.</returns>
        public ResultUser Transform(User user, IEnumerable<ILink> links)
        {
            return new ResultUser(
                user.Id,
                user.Roles,
                links);
        }
    }
}
