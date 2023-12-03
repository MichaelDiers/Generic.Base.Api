namespace Generic.Base.Api.Controllers
{
    using System.Security.Claims;
    using Generic.Base.Api.Models;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;

    /// <summary>
    ///     The base controller for crud operations.
    /// </summary>
    /// <seealso cref="Microsoft.AspNetCore.Mvc.ControllerBase" />
    public class CrudControllerBase : ControllerBase
    {
        /// <summary>
        ///     The required claims for executing an operation.
        /// </summary>
        private readonly IList<Claim> requiredClaims;

        /// <summary>
        ///     Initializes a new instance of the <see cref="CrudControllerBase" /> class.
        /// </summary>
        /// <param name="requiredClaims">The required claims.</param>
        public CrudControllerBase(IEnumerable<Claim> requiredClaims)
        {
            this.requiredClaims = requiredClaims.ToArray();
        }

        /// <summary>
        ///     An options request for the available operations of the api.
        /// </summary>
        /// <returns>A <see cref="OkObjectResult" /> with the available operations.</returns>
        /// <remarks>
        ///     Sample request:
        ///     OPTIONS /
        /// </remarks>
        /// <response code="200">If the operation succeeds.</response>
        /// <response code="401">If no api key is provided or the authentication fails.</response>
        /// <response code="403">If the api key is invalid or the user has no permission for the requested operation.</response>
        [ProducesResponseType(
            typeof(ILinkResult),
            StatusCodes.Status200OK)]
        [ProducesResponseType(
            typeof(NoContentResult),
            StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(
            typeof(NoContentResult),
            StatusCodes.Status403Forbidden)]
        [HttpOptions]
        [AllowAnonymous]
        public ActionResult Options()
        {
            return this.Ok(new LinkResult(this.CreateUrns(this.Request.Path.Value)));
        }

        /// <summary>
        ///     Creates the urns for the given url and urn.
        /// </summary>
        /// <param name="baseUrl">The base url of the urn.</param>
        /// <param name="id">The optional identifier that is added to the url.</param>
        /// <returns>An <see cref="IEnumerable{T}" /> containing the available links.</returns>
        protected IEnumerable<ClaimLink> CreateUrns(string? baseUrl, string? id = null)
        {
            if (string.IsNullOrWhiteSpace(baseUrl))
            {
                return Enumerable.Empty<ClaimLink>();
            }

            var urnNamespace = this.GetType().Name;

            IEnumerable<ClaimLink> claimLinks = new[]
            {
                ClaimLink.Create(
                    urnNamespace,
                    Urn.ReadAll,
                    baseUrl,
                    this.requiredClaims),
                ClaimLink.Create(
                    urnNamespace,
                    Urn.Options,
                    baseUrl),
                ClaimLink.Create(
                    urnNamespace,
                    Urn.Create,
                    baseUrl,
                    this.requiredClaims)
            };

            if (!string.IsNullOrWhiteSpace(id))
            {
                claimLinks = claimLinks.Concat(
                    new[]
                    {
                        ClaimLink.Create(
                            urnNamespace,
                            Urn.Delete,
                            $"{baseUrl}/{id}",
                            this.requiredClaims),
                        ClaimLink.Create(
                            urnNamespace,
                            Urn.Update,
                            $"{baseUrl}/{id}",
                            this.requiredClaims),
                        ClaimLink.Create(
                            urnNamespace,
                            Urn.ReadById,
                            $"{baseUrl}/{id}",
                            this.requiredClaims)
                    });
            }

            return claimLinks.Where(link => link.CanBeAccessed(this.User.Claims));
        }
    }
}
