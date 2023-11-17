namespace Generic.Base.Api.AuthServices.AuthService
{
    using System.ComponentModel.DataAnnotations;
    using Generic.Base.Api.Database;

    /// <summary>
    ///     Describes the data for a sign in operation.
    /// </summary>
    /// <seealso cref="IIdEntry" />
    public class SignIn : IIdEntry
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="SignIn" /> class.
        /// </summary>
        /// <param name="password">The password.</param>
        /// <param name="id">The identifier.</param>
        public SignIn(string password, string id)
        {
            this.Password = password;
            this.Id = id;
        }

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
