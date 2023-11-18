namespace Generic.Base.Api.Controllers
{
    using Generic.Base.Api.Models;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    /// <summary>
    ///     Controller that provides <see cref="HttpMethod.Options" />.
    /// </summary>
    /// <seealso cref="ControllerBase" />
    public abstract class OptionsControllerBase : ControllerBase
    {
        /// <summary>
        ///     The links the controller provides.
        /// </summary>
        private readonly IEnumerable<IClaimLink> links;

        /// <summary>
        ///     Initializes a new instance of the <see cref="OptionsControllerBase" /> class.
        /// </summary>
        /// <param name="links">The links the controller provides.</param>
        protected OptionsControllerBase(params IClaimLink[] links)
        {
            this.links = links;
        }

        /// <summary>
        ///     Provides the links to the operations of the controller.
        /// </summary>
        /// <param name="cancellationToken">Indicates that the start process has been aborted.</param>
        /// <returns>A <see cref="LinkResult" /> of links the current user can access.</returns>
        [HttpOptions]
        [AllowAnonymous]
        public LinkResult Options(CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            return new LinkResult(this.links.Where(link => link.CanBeAccessed(this.User.Claims)));
        }
    }
}
