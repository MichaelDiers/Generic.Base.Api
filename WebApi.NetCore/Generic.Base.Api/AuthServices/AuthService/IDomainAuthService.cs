namespace Generic.Base.Api.AuthServices.AuthService
{
    using Generic.Base.Api.Jwt;

    /// <summary>
    ///     The domain auth service.
    /// </summary>
    public interface IDomainAuthService
    {
        /// <summary>
        ///     Changes the password of a user.
        /// </summary>
        /// <param name="changePassword">The change password data.</param>
        /// <param name="userId">The user identifier.</param>
        /// <param name="cancellationToken">Indicates that the start process has been aborted.</param>
        /// <returns>A <see cref="Task" /> whose result indicates success.</returns>
        Task ChangePasswordAsync(ChangePassword changePassword, string userId, CancellationToken cancellationToken);

        /// <summary>
        ///     Deletes the current user.
        /// </summary>
        /// <param name="signIn">The sign in data.</param>
        /// <param name="userId">The user identifier from the current user.</param>
        /// <param name="cancellationToken">Indicates that the start process has been aborted.</param>
        /// <returns>A <see cref="Task" /> whose result indicates success.</returns>
        Task DeleteAsync(SignIn signIn, string userId, CancellationToken cancellationToken);

        /// <summary>
        ///     Create a new access token.
        /// </summary>
        /// <param name="refreshToken">The refresh token.</param>
        /// <param name="userId">The identifier of the current user.</param>
        /// <param name="cancellationToken">Indicates that the start process has been aborted.</param>
        /// <returns>
        ///     A <see cref="Task{T}" /> whose result contains the given <paramref name="refreshToken" /> and a new access
        ///     token.
        /// </returns>
        Task<IToken> RefreshAsync(string refreshToken, string userId, CancellationToken cancellationToken);

        /// <summary>
        ///     Sign in an existing user.
        /// </summary>
        /// <param name="signIn">The sign in data.</param>
        /// <param name="cancellationToken">Indicates that the start process has been aborted.</param>
        /// <returns>A <see cref="Task{T}" /> whose result are access and refresh token.</returns>
        Task<IToken> SignInAsync(SignIn signIn, CancellationToken cancellationToken);

        /// <summary>
        ///     Signs up a new user.
        /// </summary>
        /// <param name="signUp">The sign up data.</param>
        /// <param name="cancellationToken">Indicates that the start process has been aborted.</param>
        /// <returns>A <see cref="Task{T}" /> whose result are access and refresh token.</returns>
        Task<IToken> SignUpAsync(SignUp signUp, CancellationToken cancellationToken);
    }
}
