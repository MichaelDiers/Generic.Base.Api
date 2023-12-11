namespace Generic.Base.Api.Extensions
{
    using System.Security.Claims;

    /// <summary>
    ///     Extensions for <see cref="Claim" /> and <see cref="IEnumerable{T}" /> of <see cref="Claim" />.
    /// </summary>
    public static class ClaimExtensions
    {
        /// <summary>
        ///     Tries to get the user identifier.
        /// </summary>
        /// <param name="claims">The claims.</param>
        /// <returns>The user id.</returns>
        /// <exception cref="ArgumentException">Is thrown if the user id is not found.</exception>
        public static string GetUserId(this IEnumerable<Claim> claims)
        {
            if (!claims.TryGetUserId(out var userId))
            {
                throw new ArgumentException("Cannot find user id.");
            }

            return userId;
        }

        /// <summary>
        ///     Determines whether a claim is a refresh token identifier.
        /// </summary>
        /// <param name="claim">The claim.</param>
        /// <returns>
        ///     <c>true</c> if the claim is a refresh token identifier; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsRefreshTokenId(this Claim claim)
        {
            return claim.Type == Constants.RefreshTokenIdClaimType;
        }

        /// <summary>
        ///     Determines whether a claim is a user identifier.
        /// </summary>
        /// <param name="claim">The claim.</param>
        /// <returns>
        ///     <c>true</c> if the claim is a user identifier; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsUserId(this Claim claim)
        {
            return claim.Type == Constants.UserIdClaimType;
        }

        /// <summary>
        ///     Tries to get the refresh token identifier.
        /// </summary>
        /// <param name="claims">The claims.</param>
        /// <param name="refreshTokenId">The refresh token id.</param>
        /// <returns>True if the refresh token id is found; otherwise false.</returns>
        public static bool TryGetRefreshTokenId(this IEnumerable<Claim> claims, out string refreshTokenId)
        {
            return claims.TryGetValue(
                ClaimExtensions.IsRefreshTokenId,
                out refreshTokenId);
        }

        /// <summary>
        ///     Tries to get the user identifier.
        /// </summary>
        /// <param name="claims">The claims.</param>
        /// <param name="userId">The user id.</param>
        /// <returns>True if the user id is found; otherwise false.</returns>
        public static bool TryGetUserId(this IEnumerable<Claim> claims, out string userId)
        {
            return claims.TryGetValue(
                ClaimExtensions.IsUserId,
                out userId);
        }

        /// <summary>
        ///     Tries to get a claim value.
        /// </summary>
        /// <param name="claims">The claims.</param>
        /// <param name="isClaimType">A function that identifies the requested claim.</param>
        /// <param name="claimValue">The claim value.</param>
        /// <returns><c>true</c> if the claim type is found; otherwise, <c>false</c>.</returns>
        private static bool TryGetValue(
            this IEnumerable<Claim> claims,
            Func<Claim, bool> isClaimType,
            out string claimValue
        )
        {
            var claim = claims.FirstOrDefault(isClaimType);
            if (claim is null || string.IsNullOrWhiteSpace(claim.Value))
            {
                claimValue = string.Empty;
                return false;
            }

            claimValue = claim.Value;
            return true;
        }
    }
}
