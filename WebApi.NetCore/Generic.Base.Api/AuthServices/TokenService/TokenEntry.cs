namespace Generic.Base.Api.AuthServices.TokenService
{
    using System.ComponentModel.DataAnnotations;
    using Generic.Base.Api.Database;

    /// <summary>
    ///     Describes a token.
    /// </summary>
    /// <seealso cref="IIdEntry" />
    public class TokenEntry : IIdEntry
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="TokenEntry" /> class.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="userId">The owner of the token.</param>
        /// <param name="validUntil">The information until when the token is valid.</param>
        public TokenEntry(string id, string userId, string validUntil)
        {
            this.Id = id;
            this.UserId = userId;
            this.ValidUntil = validUntil;
        }

        /// <summary>
        ///     Gets the owner of the token.
        /// </summary>
        [Required]
        [StringLength(
            AuthServicesValidation.IdMaxLength,
            MinimumLength = AuthServicesValidation.IdMinLength)]
        public string UserId { get; }

        /// <summary>
        ///     Gets the information until when the token is valid.
        /// </summary>
        [Required]
        public string ValidUntil { get; }

        /// <summary>
        ///     Gets the identifier.
        /// </summary>
        [Required]
        [StringLength(
            AuthServicesValidation.IdMaxLength,
            MinimumLength = AuthServicesValidation.IdMinLength)]
        public string Id { get; }
    }
}
