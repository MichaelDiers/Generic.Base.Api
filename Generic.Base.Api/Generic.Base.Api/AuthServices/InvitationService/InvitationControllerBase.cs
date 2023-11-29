namespace Generic.Base.Api.AuthServices.InvitationService
{
    using System.Security.Claims;
    using Generic.Base.Api.Controllers;
    using Generic.Base.Api.Services;
    using Generic.Base.Api.Transformer;

    /// <summary>
    ///     A base controller for handling invitations.
    /// </summary>
    public abstract class InvitationControllerBase
        : CrudController<Invitation, Invitation, Invitation, ResultInvitation>
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="CrudController{TCreate, TEntry, TUpdate, TResult}" /> class.
        /// </summary>
        /// <param name="domainService">The domain service.</param>
        /// <param name="transformer">Transformer for entries used by controllers.</param>
        /// <param name="requiredClaims">The required claims for accessing the service.</param>
        protected InvitationControllerBase(
            IDomainService<Invitation, Invitation, Invitation> domainService,
            IControllerTransformer<Invitation, ResultInvitation> transformer,
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
