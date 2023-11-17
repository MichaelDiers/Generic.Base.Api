namespace Generic.Base.Api.AuthServices.AuthService
{
    using System.ComponentModel.DataAnnotations;
    using Generic.Base.Api.Database;

    /// <summary>
    ///     Describes the data for a sign up operation.
    /// </summary>
    /// <seealso cref="Generic.Base.Api.Database.IIdEntry" />
    public class SignUp : IIdEntry
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="SignUp" /> class.
        /// </summary>
        /// <param name="displayName">The display name.</param>
        /// <param name="invitationCode">The invitation code.</param>
        /// <param name="password">The password.</param>
        /// <param name="id">The identifier.</param>
        public SignUp(
            string displayName,
            string invitationCode,
            string password,
            string id
        )
        {
            this.DisplayName = displayName;
            this.InvitationCode = invitationCode;
            this.Password = password;
            this.Id = id;
        }

        /// <summary>
        ///     Gets the display name.
        /// </summary>
        [Required]
        [StringLength(
            AuthServicesValidation.DisplayNameMaxLength,
            MinimumLength = AuthServicesValidation.DisplayNameMinLength)]
        public string DisplayName { get; }

        /// <summary>
        ///     Gets the invitation code.
        /// </summary>
        [Required]
        [StringLength(
            AuthServicesValidation.InvitationCodeMaxLength,
            MinimumLength = AuthServicesValidation.InvitationCodeMinLength)]
        public string InvitationCode { get; }

        /// <summary>
        ///     Gets the password.
        /// </summary>
        [Required]
        [StringLength(
            AuthServicesValidation.PasswordMaxLength,
            MinimumLength = AuthServicesValidation.PasswordMinLength)]
        public string Password { get; }

        /// <summary>
        ///     Gets the identifier.
        /// </summary>
        [Required]
        [StringLength(
            AuthServicesValidation.UserIdMaxLength,
            MinimumLength = AuthServicesValidation.UserIdMinLength)]
        public string Id { get; }
    }
}
