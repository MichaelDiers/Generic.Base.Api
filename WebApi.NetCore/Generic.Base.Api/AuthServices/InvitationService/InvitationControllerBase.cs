namespace Generic.Base.Api.AuthServices.InvitationService
{
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
        protected InvitationControllerBase(
            IDomainService<Invitation, Invitation, Invitation> domainService,
            IControllerTransformer<Invitation, ResultInvitation> transformer
        )
            : base(
                domainService,
                transformer)
        {
        }
    }
}
