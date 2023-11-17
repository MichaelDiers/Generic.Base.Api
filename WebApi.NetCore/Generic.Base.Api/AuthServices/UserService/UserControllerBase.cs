﻿namespace Generic.Base.Api.AuthServices.UserService
{
    using Generic.Base.Api.Controllers;
    using Generic.Base.Api.Services;
    using Generic.Base.Api.Transformer;

    /// <summary>
    ///     A base controller for handling user entries.
    /// </summary>
    public abstract class UserControllerBase : CrudController<User, User, User, ResultUser>
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="UserControllerBase" /> class.
        /// </summary>
        /// <param name="domainService">The domain service.</param>
        /// <param name="transformer">Transformer for entries used by the controller.</param>
        protected UserControllerBase(
            IDomainService<User, User, User> domainService,
            IControllerTransformer<User, ResultUser> transformer
        )
            : base(
                domainService,
                transformer)
        {
        }
    }
}