﻿namespace Generic.Base.Api.Controllers
{
    using System.Security.Claims;
    using Generic.Base.Api.Database;
    using Generic.Base.Api.Exceptions;
    using Generic.Base.Api.Extensions;
    using Generic.Base.Api.Models;
    using Generic.Base.Api.Services;
    using Generic.Base.Api.Transformer;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;

    /// <summary>
    ///     Base class for crud controllers.
    /// </summary>
    /// <typeparam name="TCreate">The data for creating an instance of <typeparamref name="TEntry" />.</typeparam>
    /// <typeparam name="TEntry">The type of the entry.</typeparam>
    /// <typeparam name="TUpdate">The data for updating an instance of <typeparamref name="TEntry" />.</typeparam>
    /// <typeparam name="TResult">The type of the result that is sent to the client.</typeparam>
    /// <seealso cref="ControllerBase" />
    public abstract class UserBoundCrudController<TCreate, TEntry, TUpdate, TResult> : CrudControllerBase
        where TEntry : IIdEntry where TResult : ILinkResult
    {
        /// <summary>
        ///     Access the operations of the domain service.
        /// </summary>
        private readonly IUserBoundDomainService<TCreate, TEntry, TUpdate> domainService;

        /// <summary>
        ///     Transformer for entries used by controllers.
        /// </summary>
        private readonly IControllerTransformer<TEntry, TResult> transformer;

        /// <summary>
        ///     Initializes a new instance of the <see cref="CrudController{TCreate, TEntry, TUpdate, TResult}" /> class.
        /// </summary>
        /// <param name="domainService">The domain service.</param>
        /// <param name="transformer">Transformer for entries used by controllers.</param>
        /// <param name="requiredClaims">The required claims necessary for accessing the service.</param>
        protected UserBoundCrudController(
            IUserBoundDomainService<TCreate, TEntry, TUpdate> domainService,
            IControllerTransformer<TEntry, TResult> transformer,
            IEnumerable<Claim> requiredClaims
        )
            : base(requiredClaims)
        {
            this.domainService = domainService;
            this.transformer = transformer;
        }

        /// <summary>
        ///     Deletes an entry by its id.
        /// </summary>
        /// <param name="id" example="720007f1-b9ce-4448-9a38-2cd93b99f254">The identifier of the entry.</param>
        /// <param name="cancellationToken">Indicates that the start process has been aborted.</param>
        /// <returns>A <see cref="NoContentResult" /> if the entry is deleted.</returns>
        /// <remarks>
        ///     Sample request:
        ///     POST /720007f1-b9ce-4448-9a38-2cd93b99f254
        /// </remarks>
        /// <response code="204">If the entry is deleted.</response>
        /// <response code="401">If no api key is provided or the authentication fails.</response>
        /// <response code="403">If the api key is invalid or the user has no permission for the requested operation.</response>
        /// <response code="404">If no entry with the specified id exists.</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(
            typeof(NoContentResult),
            StatusCodes.Status204NoContent)]
        [ProducesResponseType(
            typeof(NotFoundResult),
            StatusCodes.Status404NotFound)]
        [ProducesResponseType(
            typeof(NoContentResult),
            StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(
            typeof(NoContentResult),
            StatusCodes.Status403Forbidden)]
        public async Task<ActionResult> Delete([FromRoute] string id, CancellationToken cancellationToken)
        {
            if (!this.IsIdValid(id))
            {
                throw new BadRequestException();
            }

            await this.domainService.DeleteAsync(
                this.GetRequiredUserId(),
                id,
                cancellationToken);
            return this.NoContent();
        }

        /// <summary>
        ///     Gets an entry by its id.
        /// </summary>
        /// <param name="id" example="720007f1-b9ce-4448-9a38-2cd93b99f254">The identifier of the entry.</param>
        /// <param name="cancellationToken">Indicates that the start process has been aborted.</param>
        /// <returns>A <see cref="OkObjectResult" /> if the entry is found.</returns>
        /// <remarks>
        ///     Sample request:
        ///     GET /720007f1-b9ce-4448-9a38-2cd93b99f254
        /// </remarks>
        /// <response code="200">If the entry is found.</response>
        /// <response code="401">If no api key is provided or the authentication fails.</response>
        /// <response code="403">If the api key is invalid or the user has no permission for the requested operation.</response>
        /// <response code="404">If no entry with the specified id exists.</response>
        [ProducesResponseType(
            typeof(ILinkResult),
            StatusCodes.Status200OK)]
        [ProducesResponseType(
            typeof(NotFoundResult),
            StatusCodes.Status404NotFound)]
        [ProducesResponseType(
            typeof(NoContentResult),
            StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(
            typeof(NoContentResult),
            StatusCodes.Status403Forbidden)]
        [HttpGet("{id}")]
        public async Task<ActionResult> Get([FromRoute] string id, CancellationToken cancellationToken)
        {
            if (!this.IsIdValid(id))
            {
                throw new BadRequestException();
            }

            var result = await this.domainService.ReadByIdAsync(
                this.GetRequiredUserId(),
                id,
                cancellationToken);
            return this.Ok(
                this.transformer.Transform(
                    result,
                    this.CreateUrns(
                        $"{this.Request.Path.Value}/..",
                        id)));
        }

        /// <summary>
        ///     Gets all available entries.
        /// </summary>
        /// <param name="cancellationToken">Indicates that the start process has been aborted.</param>
        /// <returns>A <see cref="OkObjectResult" /> with the found entries.</returns>
        /// <remarks>
        ///     Sample request:
        ///     GET /
        /// </remarks>
        /// <response code="200">If the operation succeeds.</response>
        /// <response code="401">If no api key is provided or the authentication fails.</response>
        /// <response code="403">If the api key is invalid or the user has no permission for the requested operation.</response>
        [ProducesResponseType(
            typeof(IEnumerable<ILinkResult>),
            StatusCodes.Status200OK)]
        [ProducesResponseType(
            typeof(NoContentResult),
            StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(
            typeof(NoContentResult),
            StatusCodes.Status403Forbidden)]
        [HttpGet]
        public async Task<ActionResult> Get(CancellationToken cancellationToken)
        {
            var result = await this.domainService.ReadAsync(
                this.GetRequiredUserId(),
                cancellationToken);
            return this.Ok(
                result.Select(
                    entry => this.transformer.Transform(
                        entry,
                        this.CreateUrns(
                            this.Request.Path.Value,
                            entry.Id))));
        }

        /// <summary>
        ///     Create a new entry.
        /// </summary>
        /// <param name="create">The data for creating a new entry.</param>
        /// <param name="cancellationToken">Indicates that the start process has been aborted.</param>
        /// <returns>A <see cref="CreatedAtActionResult" /> with the created entries.</returns>
        /// <remarks>
        ///     Sample request:
        ///     Post / { data }
        /// </remarks>
        /// <response code="201">If the entry is created.</response>
        /// <response code="401">If no api key is provided or the authentication fails.</response>
        /// <response code="403">If the api key is invalid or the user has no permission for the requested operation.</response>
        /// <response code="409">If an entry with the specified data already exists.</response>
        [ProducesResponseType(
            typeof(ILinkResult),
            StatusCodes.Status201Created)]
        [ProducesResponseType(
            typeof(NoContentResult),
            StatusCodes.Status409Conflict)]
        [ProducesResponseType(
            typeof(NoContentResult),
            StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(
            typeof(NoContentResult),
            StatusCodes.Status403Forbidden)]
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] TCreate create, CancellationToken cancellationToken)
        {
            var result = await this.domainService.CreateAsync(
                this.GetRequiredUserId(),
                create,
                cancellationToken);

            return this.CreatedAtAction(
                nameof(this.Get),
                new {id = result.Id},
                this.transformer.Transform(
                    result,
                    this.CreateUrns(
                        this.Request.Path.Value,
                        result.Id)));
        }

        /// <summary>
        ///     Update an entry with the given <paramref name="id" />.
        /// </summary>
        /// <param name="id">The id of the entry to be updated.</param>
        /// <param name="updateEntry">The data for updating the entry.</param>
        /// <param name="cancellationToken">Indicates that the start process has been aborted.</param>
        /// <returns>A <see cref="CreatedAtActionResult" /> with the created entries.</returns>
        /// <remarks>
        ///     Sample request:
        ///     PUT /720007f1-b9ce-4448-9a38-2cd93b99f254 { data }
        /// </remarks>
        /// <response code="204">If the entry is updated.</response>
        /// <response code="401">If no api key is provided or the authentication fails.</response>
        /// <response code="403">If the api key is invalid or the user has no permission for the requested operation.</response>
        [ProducesResponseType(
            typeof(ILinkResult),
            StatusCodes.Status204NoContent)]
        [ProducesResponseType(
            typeof(NoContentResult),
            StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(
            typeof(NoContentResult),
            StatusCodes.Status403Forbidden)]
        [HttpPut("{id}")]
        public async Task<ActionResult> Put(
            [FromRoute] string id,
            [FromBody] TUpdate updateEntry,
            CancellationToken cancellationToken
        )
        {
            if (!this.IsIdValid(id))
            {
                throw new BadRequestException();
            }

            await this.domainService.UpdateAsync(
                updateEntry,
                this.GetRequiredUserId(),
                id,
                cancellationToken);
            return this.NoContent();
        }

        /// <summary>
        ///     Determines whether the specified identifier is valid.
        /// </summary>
        /// <param name="id">The identifier to be checked.</param>
        /// <returns>
        ///     <c>true</c> if the specified identifier is valid; otherwise, <c>false</c>.
        /// </returns>
        protected abstract bool IsIdValid(string id);

        /// <summary>
        ///     Gets the required user identifier from the claims of the user.
        /// </summary>
        /// <returns>The id of the user.</returns>
        /// <exception cref="UnauthorizedException">Thrown if no user id claim is available.</exception>
        private string GetRequiredUserId()
        {
            try
            {
                var userId = this.User.Claims.GetUserId();
                if (this.IsIdValid(userId))
                {
                    return userId;
                }

                throw new UnauthorizedException();
            }
            catch (ArgumentException)
            {
                throw new UnauthorizedException();
            }
        }
    }
}
