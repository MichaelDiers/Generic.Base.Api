namespace Generic.Base.Api.AuthServices.TokenService
{
    using System.Security.Claims;
    using Generic.Base.Api.Controllers;
    using Generic.Base.Api.Services;
    using Generic.Base.Api.Transformer;

    /// <summary>
    ///     A base controller for handling token entries.
    /// </summary>
    public abstract class TokenEntryControllerBase
        : CrudController<TokenEntry, TokenEntry, TokenEntry, ResultTokenEntry>
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="TokenEntryControllerBase" /> class.
        /// </summary>
        /// <param name="domainService">The domain service.</param>
        /// <param name="transformer">Transformer for entries used by controllers.</param>
        /// <param name="requiredClaims">The required claims for accessing the service.</param>
        protected TokenEntryControllerBase(
            IDomainService<TokenEntry, TokenEntry, TokenEntry> domainService,
            IControllerTransformer<TokenEntry, ResultTokenEntry> transformer,
            IEnumerable<Claim> requiredClaims
        )
            : base(
                domainService,
                transformer,
                requiredClaims)
        {
        }

        /// <summary>
        ///     Determines whether the specified identifier is valid.
        /// </summary>
        /// <param name="id">The identifier to be checked.</param>
        /// <returns>
        ///     <c>true</c> if the specified identifier is valid; otherwise, <c>false</c>.
        /// </returns>
        protected override bool IsIdValid(string id)
        {
            return !string.IsNullOrWhiteSpace(id) &&
                   id.Length is >= AuthServicesValidation.IdMinLength and <= AuthServicesValidation.IdMaxLength;
        }
    }
}
