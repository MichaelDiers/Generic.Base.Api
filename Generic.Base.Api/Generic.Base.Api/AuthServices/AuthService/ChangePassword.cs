namespace Generic.Base.Api.AuthServices.AuthService
{
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    ///     The data for changing the password of a user.
    /// </summary>
    public class ChangePassword
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="ChangePassword" /> class.
        /// </summary>
        /// <param name="oldPassword">The old password.</param>
        /// <param name="newPassword">The new password.</param>
        public ChangePassword(string oldPassword, string newPassword)
        {
            this.OldPassword = oldPassword;
            this.NewPassword = newPassword;
        }

        /// <summary>
        ///     Gets the new password.
        /// </summary>
        [Required]
        [StringLength(
            AuthServicesValidation.PasswordMaxLength,
            MinimumLength = AuthServicesValidation.PasswordMinLength)]
        public string NewPassword { get; }

        /// <summary>
        ///     Gets the old password.
        /// </summary>
        [Required]
        [StringLength(
            AuthServicesValidation.PasswordMaxLength,
            MinimumLength = AuthServicesValidation.PasswordMinLength)]
        public string OldPassword { get; }
    }
}
