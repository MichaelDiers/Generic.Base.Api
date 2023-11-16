﻿namespace Generic.Base.Api.Controllers
{
    using Generic.Base.Api.Database;
    using Generic.Base.Api.Models;
    using Generic.Base.Api.Result;
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
    public class CrudController<TCreate, TEntry, TUpdate, TResult> : ControllerBase
        where TEntry : IIdEntry where TResult : ILinkResult
    {
        /// <summary>
        ///     Access the operations of the domain service.
        /// </summary>
        private readonly IDomainService<TCreate, TEntry, TUpdate> domainService;

        /// <summary>
        ///     Transformer for entries used by controllers.
        /// </summary>
        private readonly IControllerTransformer<TEntry, TResult> transformer;

        /// <summary>
        ///     Initializes a new instance of the <see cref="CrudController{TCreate, TEntry, TUpdate, TResult}" /> class.
        /// </summary>
        /// <param name="domainService">The domain service.</param>
        /// <param name="transformer">Transformer for entries used by controllers.</param>
        protected CrudController(
            IDomainService<TCreate, TEntry, TUpdate> domainService,
            IControllerTransformer<TEntry, TResult> transformer
        )
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
            await this.domainService.DeleteAsync(
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
            var result = await this.domainService.ReadByIdAsync(
                id,
                cancellationToken);
            return this.Ok(
                this.transformer.Transform(
                    result,
                    this.CreateUrns($"{this.Request.Path.Value}/..")));
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
            var result = await this.domainService.ReadAsync(cancellationToken);
            return this.Ok(
                result.Select(
                    entry => this.transformer.Transform(
                        entry,
                        this.CreateUrns(
                            this.Request.Path.Value,
                            entry.Id))));
        }

        /// <summary>
        ///     An options request for the available operations of the api.
        /// </summary>
        /// <returns>A <see cref="OkObjectResult" /> with the availble operations.</returns>
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
        public ActionResult Options()
        {
            return this.Ok(new LinkResult(this.CreateUrns(this.Request.Path.Value)));
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
            await this.domainService.UpdateAsync(
                updateEntry,
                id,
                cancellationToken);
            return this.NoContent();
        }

        /// <summary>
        ///     Creates the urns for the given url and urn.
        /// </summary>
        /// <param name="baseUrl">The base url of the urn.</param>
        /// <param name="id">The optional identifier that is added to the url.</param>
        /// <returns>An <see cref="IEnumerable{T}" /> containing the available links.</returns>
        private IEnumerable<ILink> CreateUrns(string? baseUrl, string? id = null)
        {
            if (string.IsNullOrWhiteSpace(baseUrl))
            {
                return Enumerable.Empty<ILink>();
            }

            var result = new List<ILink>();

            if (string.IsNullOrWhiteSpace(id))
            {
                result.Add(
                    new Link(
                        Urn.Options,
                        $"{baseUrl}"));
                result.Add(
                    new Link(
                        Urn.Create,
                        $"{baseUrl}"));
                result.Add(
                    new Link(
                        Urn.ReadAll,
                        $"{baseUrl}"));
            }
            else
            {
                result.Add(
                    new Link(
                        Urn.ReadById,
                        $"{baseUrl}/{id}"));
                result.Add(
                    new Link(
                        Urn.Update,
                        $"{baseUrl}/{id}"));
                result.Add(
                    new Link(
                        Urn.Delete,
                        $"{baseUrl}/{id}"));
            }

            return result;
        }
    }
}
