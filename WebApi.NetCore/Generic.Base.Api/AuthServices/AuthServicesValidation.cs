﻿namespace Generic.Base.Api.AuthServices
{
    /// <summary>
    ///     Specification for auth services data.
    /// </summary>
    internal static class AuthServicesValidation
    {
        /// <summary>
        ///     The display name maximum length.
        /// </summary>
        public const int DisplayNameMaxLength = 100;

        /// <summary>
        ///     The display name minimum length.
        /// </summary>
        public const int DisplayNameMinLength = 2;

        /// <summary>
        ///     The invitation code maximum length.
        /// </summary>
        public const int InvitationCodeMaxLength = 100;

        /// <summary>
        ///     The invitation code minimum length.
        /// </summary>
        public const int InvitationCodeMinLength = 2;

        /// <summary>
        ///     The maximum length of an invitation id.
        /// </summary>
        public const int InvitationIdMax = 100;

        /// <summary>
        ///     The minimum length of an invitation id.
        /// </summary>
        public const int InvitationIdMin = 2;

        /// <summary>
        ///     The maximum of roles for an invitation.
        /// </summary>
        public const int InvitationRolesMax = 10;

        /// <summary>
        ///     The minimum of roles for an invitation.
        /// </summary>
        public const int InvitationRolesMin = 1;

        /// <summary>
        ///     The password maximum length.
        /// </summary>
        public const int PasswordMaxLength = 100;

        /// <summary>
        ///     The password minimum length.
        /// </summary>
        public const int PasswordMinLength = 2;

        /// <summary>
        ///     The user id maximum length.
        /// </summary>
        public const int UserIdMaxLength = 100;

        /// <summary>
        ///     The user id minimum length.
        /// </summary>
        public const int UserIdMinLength = 2;
    }
}
