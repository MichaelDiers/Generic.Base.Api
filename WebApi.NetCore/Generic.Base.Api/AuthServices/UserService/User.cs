namespace Generic.Base.Api.AuthServices.UserService
{
    using System.ComponentModel.DataAnnotations;
    using Generic.Base.Api.Database;

    /// <summary>
    ///     Describes the user data.
    /// </summary>
    /// <seealso cref="IIdEntry" />
    public class User : IIdEntry
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="User" /> class.
        /// </summary>
        /// <param name="id">The identifier of the user.</param>
        /// <param name="password">The password.</param>
        /// <param name="roles">The roles that are assigned to the user.</param>
        /// <param name="displayName">The display name.</param>
        public User(
            string id,
            string password,
            IEnumerable<Role> roles,
            string displayName
        )
        {
            this.Id = id;
            this.Password = password;
            this.DisplayName = displayName;
            this.Roles = roles.ToArray();
        }

        /// <summary>
        ///     Gets the display name.
        /// </summary>
        [Required]
        [StringLength(
            AuthServicesValidation.DisplayNameMaxLength,
            MinimumLength = AuthServicesValidation.DisplayNameMinLength)]
        public string DisplayName { get; set; }

        /// <summary>
        ///     Gets or sets the password.
        /// </summary>
        [Required]
        [StringLength(
            AuthServicesValidation.PasswordMaxLength,
            MinimumLength = AuthServicesValidation.PasswordMinLength)]
        public string Password { get; set; }

        /// <summary>
        ///     Gets the roles that are assigned to the user.
        /// </summary>
        [Required]
        [MinLength(AuthServicesValidation.InvitationRolesMin)]
        [MaxLength(AuthServicesValidation.InvitationRolesMax)]
        public IEnumerable<Role> Roles { get; set; }

        /// <summary>
        ///     Gets the identifier of the user.
        /// </summary>
        [Required]
        [StringLength(
            AuthServicesValidation.IdMaxLength,
            MinimumLength = AuthServicesValidation.IdMinLength)]
        public string Id { get; set; }
    }
}
