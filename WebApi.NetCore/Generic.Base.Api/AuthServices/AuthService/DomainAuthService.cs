namespace Generic.Base.Api.AuthServices.AuthService
{
    using System.IdentityModel.Tokens.Jwt;
    using System.Security.Claims;
    using Generic.Base.Api.AuthServices.InvitationService;
    using Generic.Base.Api.AuthServices.TokenService;
    using Generic.Base.Api.AuthServices.UserService;
    using Generic.Base.Api.Database;
    using Generic.Base.Api.Exceptions;
    using Generic.Base.Api.HashService;
    using Generic.Base.Api.Jwt;
    using Generic.Base.Api.Services;

    /// <inheritdoc cref="IDomainAuthService" />
    internal class DomainAuthService<TClientSessionHandle> : IDomainAuthService
    {
        /// <summary>
        ///     The token valid to format.
        /// </summary>
        private const string TokenValidToFormat = "yyyy.MM.dd - HH:mm:ss";

        /// <summary>
        ///     The atomic invitation service.
        /// </summary>
        private readonly IAtomicService<Invitation, Invitation, Invitation, TClientSessionHandle>
            atomicInvitationService;

        /// <summary>
        ///     The atomic token entry service.
        /// </summary>
        private readonly IAtomicService<TokenEntry, TokenEntry, TokenEntry, TClientSessionHandle>
            atomicTokenEntryService;

        /// <summary>
        ///     The atomic user service for user handling.
        /// </summary>
        private readonly IAtomicService<User, User, User, TClientSessionHandle> atomicUserService;

        /// <summary>
        ///     The hash service for passwords.
        /// </summary>
        private readonly IHashService hashService;

        /// <summary>
        ///     The jwt token service.
        /// </summary>
        private readonly IJwtTokenService jwtTokenService;

        /// <summary>
        ///     The database transaction handler.
        /// </summary>
        private readonly ITransactionHandler<TClientSessionHandle> transactionHandler;

        public DomainAuthService(
            IAtomicService<User, User, User, TClientSessionHandle> atomicUserService,
            ITransactionHandler<TClientSessionHandle> transactionHandler,
            IHashService hashService,
            IAtomicService<Invitation, Invitation, Invitation, TClientSessionHandle> atomicInvitationService,
            IJwtTokenService jwtTokenService,
            IAtomicService<TokenEntry, TokenEntry, TokenEntry, TClientSessionHandle> atomicTokenEntryService
        )
        {
            this.atomicUserService = atomicUserService;
            this.transactionHandler = transactionHandler;
            this.hashService = hashService;
            this.atomicInvitationService = atomicInvitationService;
            this.jwtTokenService = jwtTokenService;
            this.atomicTokenEntryService = atomicTokenEntryService;
        }

        /// <summary>
        ///     Changes the password of a user.
        /// </summary>
        /// <param name="changePassword">The change password data.</param>
        /// <param name="userId">The user identifier.</param>
        /// <param name="token">The access token of the request.</param>
        /// <param name="cancellationToken">Indicates that the start process has been aborted.</param>
        /// <returns>A <see cref="Task{T}" /> whose result are the created tokens.</returns>
        public async Task<IToken> ChangePasswordAsync(
            ChangePassword changePassword,
            string userId,
            string token,
            CancellationToken cancellationToken
        )
        {
            DomainAuthService<TClientSessionHandle>.CheckData(changePassword);
            using var session = await this.transactionHandler.StartTransactionAsync(cancellationToken);
            try
            {
                var user = await this.ReadUser(
                    userId,
                    cancellationToken,
                    session);
                this.CheckPassword(
                    user,
                    changePassword.OldPassword);
                await this.UpdateUser(
                    user,
                    changePassword.NewPassword,
                    cancellationToken,
                    session);

                var tokens = await this.RefreshAsync(
                    token,
                    userId,
                    cancellationToken,
                    session);
                await session.CommitTransactionAsync(cancellationToken);
                return tokens;
            }
            catch
            {
                await session.AbortTransactionAsync(cancellationToken);
                throw;
            }
        }

        /// <summary>
        ///     Deletes the current user.
        /// </summary>
        /// <param name="signIn">The sign in data.</param>
        /// <param name="userId">The user identifier from the current user.</param>
        /// <param name="cancellationToken">Indicates that the start process has been aborted.</param>
        /// <returns>A <see cref="Task" /> whose result indicates success.</returns>
        public async Task DeleteAsync(SignIn signIn, string userId, CancellationToken cancellationToken)
        {
            DomainAuthService<TClientSessionHandle>.CheckData(
                signIn,
                userId);
            using var session = await this.transactionHandler.StartTransactionAsync(cancellationToken);
            try
            {
                var user = await this.ReadUser(
                    userId,
                    cancellationToken,
                    session);
                this.CheckPassword(
                    user,
                    signIn.Password);
                await this.DeleteUser(
                    user.Id,
                    cancellationToken,
                    session);
                await session.CommitTransactionAsync(cancellationToken);
            }
            catch
            {
                await session.AbortTransactionAsync(cancellationToken);
                throw;
            }
        }

        /// <summary>
        ///     Create a new access token.
        /// </summary>
        /// <param name="refreshToken">The refresh token.</param>
        /// <param name="userId">The identifier of the current user.</param>
        /// <param name="cancellationToken">Indicates that the start process has been aborted.</param>
        /// <returns>
        ///     A <see cref="Task{T}" /> whose result contains new refresh and access tokens.
        /// </returns>
        public async Task<IToken> RefreshAsync(string refreshToken, string userId, CancellationToken cancellationToken)
        {
            using var session = await this.transactionHandler.StartTransactionAsync(cancellationToken);
            try
            {
                var token = await this.RefreshAsync(
                    refreshToken,
                    userId,
                    cancellationToken,
                    session);

                await session.CommitTransactionAsync(cancellationToken);

                return token;
            }
            catch
            {
                await session.AbortTransactionAsync(cancellationToken);
                throw;
            }
        }

        /// <summary>
        ///     Sign in an existing user.
        /// </summary>
        /// <param name="signIn">The sign in data.</param>
        /// <param name="cancellationToken">Indicates that the start process has been aborted.</param>
        /// <returns>A <see cref="Task{T}" /> whose result are access and refresh token.</returns>
        public async Task<IToken> SignInAsync(SignIn signIn, CancellationToken cancellationToken)
        {
            using var session = await this.transactionHandler.StartTransactionAsync(cancellationToken);
            try
            {
                var user = await this.ReadUser(
                    signIn.Id,
                    cancellationToken,
                    session);
                this.CheckPassword(
                    user,
                    signIn.Password);
                var token = this.CreateToken(user);
                await this.SaveRefreshToken(
                    token.RefreshToken,
                    cancellationToken,
                    session);
                await session.CommitTransactionAsync(cancellationToken);
                return token;
            }
            catch (Exception)
            {
                await session.AbortTransactionAsync(cancellationToken);
                throw;
            }
        }

        /// <summary>
        ///     Signs up a new user.
        /// </summary>
        /// <param name="signUp">The sign up data.</param>
        /// <param name="cancellationToken">Indicates that the start process has been aborted.</param>
        /// <returns>A <see cref="Task{T}" /> whose result are access and refresh token.</returns>
        public async Task<IToken> SignUpAsync(SignUp signUp, CancellationToken cancellationToken)
        {
            using var session = await this.transactionHandler.StartTransactionAsync(cancellationToken);
            try
            {
                await this.CheckUserExistence(
                    signUp.Id,
                    cancellationToken,
                    session);
                var invitation = await this.ReadInvitation(
                    signUp.InvitationCode,
                    cancellationToken,
                    session);
                var user = await this.CreateUser(
                    signUp,
                    invitation,
                    cancellationToken,
                    session);

                var token = this.CreateToken(user);
                await this.DeleteInvitation(
                    invitation.Id,
                    cancellationToken,
                    session);

                await this.SaveRefreshToken(
                    token.RefreshToken,
                    cancellationToken,
                    session);

                await session.CommitTransactionAsync(cancellationToken);
                return token;
            }
            catch (Exception)
            {
                await session.AbortTransactionAsync(cancellationToken);
                throw;
            }
        }

        /// <summary>
        ///     Checks the data.
        /// </summary>
        /// <param name="idEntry">The sign in data.</param>
        /// <param name="userId">The user identifier from the current user.</param>
        /// <exception cref="BadRequestException"></exception>
        private static void CheckData(IIdEntry idEntry, string userId)
        {
            if (!string.Equals(
                    idEntry.Id,
                    userId,
                    StringComparison.OrdinalIgnoreCase))
            {
                throw new BadRequestException();
            }
        }

        /// <summary>
        ///     Checks the data.
        /// </summary>
        /// <param name="changePassword">The change password data.</param>
        /// <exception cref="BadRequestException"></exception>
        private static void CheckData(ChangePassword changePassword)
        {
            if (changePassword.NewPassword == changePassword.OldPassword)
            {
                throw new BadRequestException();
            }
        }

        /// <summary>
        ///     Checks if the passwords <see cref="User.Password" /> and <paramref name="password" /> do match.
        /// </summary>
        /// <param name="user">The user including the hashed password.</param>
        /// <param name="password">The not hashed password.</param>
        /// <exception cref="UnauthorizedException"></exception>
        private void CheckPassword(User user, string password)
        {
            if (!this.hashService.Verify(
                    password,
                    user.Password))
            {
                throw new UnauthorizedException();
            }
        }

        /// <summary>
        ///     Checks the user existence.
        /// </summary>
        /// <param name="id">The identifier of the user.</param>
        /// <param name="cancellationToken">Indicates that the start process has been aborted.</param>
        /// <param name="transactionHandle">The database transaction handle.</param>
        /// <exception cref="ConflictException"></exception>
        private async Task CheckUserExistence(
            string id,
            CancellationToken cancellationToken,
            ITransactionHandle<TClientSessionHandle> transactionHandle
        )
        {
            try
            {
                // check if user already exists
                await this.atomicUserService.ReadByIdAsync(
                    id,
                    cancellationToken,
                    transactionHandle);
                throw new ConflictException();
            }
            catch (NotFoundException)
            {
                // user does not exists
            }
        }

        /// <summary>
        ///     Creates the access and refresh tokens.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <returns>The created tokens.</returns>
        private IToken CreateToken(User user)
        {
            return this.jwtTokenService.CreateToken(
                user.Id,
                user.DisplayName,
                user.Roles.Select(
                    role => new Claim(
                        ClaimTypes.Role,
                        role.ToString())));
        }

        /// <summary>
        ///     Creates the user.
        /// </summary>
        /// <param name="signUp">The sign up data.</param>
        /// <param name="invitation">The invitation of the user.</param>
        /// <param name="cancellationToken">Indicates that the start process has been aborted.</param>
        /// <param name="transactionHandle">The database transaction handle.</param>
        /// <returns>A <see cref="Task{T}" /> whose result is the created user.</returns>
        private async Task<User> CreateUser(
            SignUp signUp,
            Invitation invitation,
            CancellationToken cancellationToken,
            ITransactionHandle<TClientSessionHandle> transactionHandle
        )
        {
            var user = new User(
                signUp.Id,
                this.hashService.Hash(signUp.Password),
                invitation.Roles,
                signUp.DisplayName);

            return await this.atomicUserService.CreateAsync(
                user,
                cancellationToken,
                transactionHandle);
        }

        /// <summary>
        ///     Deletes the invitation.
        /// </summary>
        /// <param name="id">The identifier of the invitation.</param>
        /// <param name="cancellationToken">Indicates that the start process has been aborted.</param>
        /// <param name="transactionHandle">The database transaction handle.</param>
        private async Task DeleteInvitation(
            string id,
            CancellationToken cancellationToken,
            ITransactionHandle<TClientSessionHandle> transactionHandle
        )
        {
            await this.atomicInvitationService.DeleteAsync(
                id,
                cancellationToken,
                transactionHandle);
        }

        /// <summary>
        ///     Deletes a user by it id.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="cancellationToken">Indicates that the start process has been aborted.</param>
        /// <param name="transactionHandle">The database transaction handle.</param>
        private async Task DeleteUser(
            string userId,
            CancellationToken cancellationToken,
            ITransactionHandle<TClientSessionHandle> transactionHandle
        )
        {
            await this.atomicUserService.DeleteAsync(
                userId,
                cancellationToken,
                transactionHandle);
        }

        /// <summary>
        ///     Reads the invitation of the user.
        /// </summary>
        /// <param name="id">The identifier of the invitation.</param>
        /// <param name="cancellationToken">Indicates that the start process has been aborted.</param>
        /// <param name="transactionHandle">The database transaction handle.</param>
        /// <returns>A <see cref="Task{T}" /> whose result is the found invitation.</returns>
        /// <exception cref="UnauthorizedException"></exception>
        private async Task<Invitation> ReadInvitation(
            string id,
            CancellationToken cancellationToken,
            ITransactionHandle<TClientSessionHandle> transactionHandle
        )
        {
            try
            {
                return await this.atomicInvitationService.ReadByIdAsync(
                    id,
                    cancellationToken,
                    transactionHandle);
            }
            catch (NotFoundException)
            {
                // user has no valid invitation
                throw new UnauthorizedException();
            }
        }

        /// <summary>
        ///     Reads the jwt security token.
        /// </summary>
        /// <param name="token">The token.</param>
        /// <returns>An instance of <see cref="JwtSecurityToken" />.</returns>
        private static JwtSecurityToken ReadJwtSecurityToken(string token)
        {
            return new JwtSecurityTokenHandler().ReadJwtToken(token);
        }

        /// <summary>
        ///     Reads the refresh token identifier.
        /// </summary>
        /// <param name="token">The token.</param>
        /// <returns>The id of the refresh token.</returns>
        private static string ReadRefreshTokenId(JwtSecurityToken token)
        {
            return token.Claims.First(claim => claim.Type == Constants.RefreshTokenIdClaimType).Value;
        }

        /// <summary>
        ///     Reads the user.
        /// </summary>
        /// <param name="id">The identifier if the user.</param>
        /// <param name="cancellationToken">Indicates that the start process has been aborted.</param>
        /// <param name="transactionHandle">The database transaction handle.</param>
        /// <returns>A <see cref="Task{T}" /> whose result is the found user.</returns>
        private async Task<User> ReadUser(
            string id,
            CancellationToken cancellationToken,
            ITransactionHandle<TClientSessionHandle> transactionHandle
        )
        {
            return await this.atomicUserService.ReadByIdAsync(
                id,
                cancellationToken,
                transactionHandle);
        }

        /// <summary>
        ///     Create a new access token.
        /// </summary>
        /// <param name="refreshToken">The refresh token.</param>
        /// <param name="userId">The identifier of the current user.</param>
        /// <param name="cancellationToken">Indicates that the start process has been aborted.</param>
        /// <param name="session">The database transaction handle.</param>
        /// <returns>
        ///     A <see cref="Task{T}" /> whose result contains new refresh and access tokens.
        /// </returns>
        private async Task<IToken> RefreshAsync(
            string refreshToken,
            string userId,
            CancellationToken cancellationToken,
            ITransactionHandle<TClientSessionHandle> session
        )
        {
            await this.RevokeRefreshToken(
                refreshToken,
                cancellationToken,
                session);

            var user = await this.ReadUser(
                userId,
                cancellationToken,
                session);

            return this.CreateToken(user);
        }

        /// <summary>
        ///     Checks the refresh token.
        /// </summary>
        /// <param name="refreshToken">The refresh token.</param>
        /// <param name="cancellationToken">Indicates that the start process has been aborted.</param>
        /// <param name="transactionHandle">The database transaction handle.</param>
        /// <returns>A <see cref="Task" /> whose result indicates success.</returns>
        private async Task RevokeRefreshToken(
            string refreshToken,
            CancellationToken cancellationToken,
            ITransactionHandle<TClientSessionHandle> transactionHandle
        )
        {
            var token = DomainAuthService<TClientSessionHandle>.ReadJwtSecurityToken(refreshToken);
            var refreshTokenId = DomainAuthService<TClientSessionHandle>.ReadRefreshTokenId(token);
            await this.atomicTokenEntryService.DeleteAsync(
                refreshTokenId,
                cancellationToken,
                transactionHandle);
        }

        /// <summary>
        ///     Saves the refresh token.
        /// </summary>
        /// <param name="refreshToken">The refresh token.</param>
        /// <param name="cancellationToken">Indicates that the start process has been aborted.</param>
        /// <param name="transactionHandle">The database transaction handle.</param>
        private async Task SaveRefreshToken(
            string refreshToken,
            CancellationToken cancellationToken,
            ITransactionHandle<TClientSessionHandle> transactionHandle
        )
        {
            var token = DomainAuthService<TClientSessionHandle>.ReadJwtSecurityToken(refreshToken);

            var refreshTokenId = token.Claims.First(claim => claim.Type == Constants.RefreshTokenIdClaimType).Value;
            var userId = token.Claims.First(claim => claim.Type == ClaimTypes.NameIdentifier).Value;
            var validTo = token.ValidTo.ToString(DomainAuthService<TClientSessionHandle>.TokenValidToFormat);

            await this.atomicTokenEntryService.CreateAsync(
                new TokenEntry(
                    refreshTokenId,
                    userId,
                    validTo),
                cancellationToken,
                transactionHandle);
        }

        /// <summary>
        ///     Updates the user and sets the new password.
        /// </summary>
        /// <param name="user">The user to be updated.</param>
        /// <param name="newPassword">The new password.</param>
        /// <param name="cancellationToken">Indicates that the start process has been aborted.</param>
        /// <param name="transactionHandle">The database transaction handle.</param>
        /// <returns>A <see cref="Task" /> whose result indicates success.</returns>
        private async Task UpdateUser(
            User user,
            string newPassword,
            CancellationToken cancellationToken,
            ITransactionHandle<TClientSessionHandle> transactionHandle
        )
        {
            user.Password = this.hashService.Hash(newPassword);
            await this.atomicUserService.UpdateAsync(
                user,
                user.Id,
                cancellationToken,
                transactionHandle);
        }
    }
}
