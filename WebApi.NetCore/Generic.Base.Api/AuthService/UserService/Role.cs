namespace Generic.Base.Api.AuthService.UserService
{
    using System.Text.Json.Serialization;

    /// <summary>
    ///     The supported roles of the user.
    /// </summary>
    [JsonConverter(typeof(JsonStringEnumConverter))]
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
        Admin,

        /// <summary>
        ///     The guest role.
        /// </summary>
        Guest,

        /// <summary>
        ///     The refresher role.
        /// </summary>
        Refresher,

        /// <summary>
        ///     The accessor role.
        /// </summary>
        Accessor
    }
}
