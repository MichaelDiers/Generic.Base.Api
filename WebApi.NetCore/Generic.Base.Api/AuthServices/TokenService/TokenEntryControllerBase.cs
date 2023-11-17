namespace Generic.Base.Api.AuthServices.TokenService
{
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
        protected TokenEntryControllerBase(
            IDomainService<TokenEntry, TokenEntry, TokenEntry> domainService,
            IControllerTransformer<TokenEntry, ResultTokenEntry> transformer
        )
            : base(
                domainService,
                transformer)
        {
        }
    }
}
