namespace Generic.Base.Api.AuthServices.AuthService
{
    using Generic.Base.Api.Jwt;

    /// <summary>
    ///     The domain auth service.
    /// </summary>
    public interface IDomainAuthService
    {
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
