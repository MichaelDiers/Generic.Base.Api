namespace Generic.Base.Api.AuthServices.UserService
{
    using Generic.Base.Api.Models;
    using Generic.Base.Api.Transformer;

    /// <summary>
    ///     Transformer of users.
    /// </summary>
    internal class UserTransformer : IControllerTransformer<User, ResultUser>, IAtomicTransformer<User, User, User>
    {
        /// <summary>
        ///     Transforms the specified <paramref name="user" /> of type <see cref="User" /> to
        ///     <see cref="User" />.
        /// </summary>
        /// <param name="user">The data for creating an entry of type <see cref="User" />.</param>
        /// <returns>The transformed entry of type <see cref="User" />.</returns>
        public User Transform(User user)
        {
            return user;
        }

        /// <summary>
        ///     Transforms the specified <paramref name="user" /> of type <see cref="User" /> to
        ///     <see cref="User" />.
        /// </summary>
        /// <param name="user">The data for creating an entry of type <see cref="User" />.</param>
        /// <param name="id">The id of the entry to be updated.</param>
        /// <returns>The transformed entry of type <see cref="User" />.</returns>
        public User Transform(User user, string id)
        {
            return user;
        }

        /// <summary>
        ///     Transforms the specified entry of <see cref="User" /> to <see cref="ResultUser" />.
        /// </summary>
        /// <param name="user">The data for creating an instance of <see cref="ResultUser" />.</param>
        /// <param name="links">The urn that describe the supported operations on the result entry.</param>
        /// <returns>The transformed entry of type <see cref="User" />.</returns>
        public ResultUser Transform(User user, IEnumerable<Link> links)
        {
            return new ResultUser(
                user.Id,
                user.Roles,
                links,
                user.DisplayName);
        }
    }
}
