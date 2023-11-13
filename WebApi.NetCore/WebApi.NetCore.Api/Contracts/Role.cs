namespace WebApi.NetCore.Api.Contracts
{
    /// <summary>
    ///     The roles of the application.
    /// </summary>
    public enum Role
    {
        /// <summary>
        ///     The undefined role.
        /// </summary>
        None = 0,

        /// <summary>
        ///     The user role.
        /// </summary>
        User,

        /// <summary>
        ///     The admin role.
        /// </summary>
        Admin
    }
}
