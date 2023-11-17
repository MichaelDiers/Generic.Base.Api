namespace Generic.Base.Api.AuthServices.InvitationService
{
    using Generic.Base.Api.Result;
    using Generic.Base.Api.Transformer;

    internal class InvitationTransformer
        : IControllerTransformer<Invitation, ResultInvitation>, IAtomicTransformer<Invitation, Invitation, Invitation>
    {
        /// <summary>
        ///     Transforms the specified <paramref name="createEntry" /> of type <see cref="Invitation" /> to
        ///     <see cref="Invitation" />.
        /// </summary>
        /// <param name="createEntry">The data for creating an entry of type <see cref="Invitation" />.</param>
        /// <returns>The transformed entry of type <see cref="Invitation" />.</returns>
        public Invitation Transform(Invitation createEntry)
        {
            return createEntry;
        }

        /// <summary>
        ///     Transforms the specified <paramref name="updateEntry" /> of type <see cref="Invitation" /> to
        ///     <see cref="Invitation" />.
        /// </summary>
        /// <param name="updateEntry">The data for creating an entry of type <see cref="Invitation" />.</param>
        /// <param name="id">The id of the entry to be updated.</param>
        /// <returns>The transformed entry of type <see cref="Invitation" />.</returns>
        public Invitation Transform(Invitation updateEntry, string id)
        {
            return updateEntry;
        }

        /// <summary>
        ///     Transforms the specified entry of <see cref="Invitation" /> to <see cref="ResultInvitation" />.
        /// </summary>
        /// <param name="entry">The data for creating an instance of <see cref="ResultInvitation" />.</param>
        /// <param name="links">The urn that describe the supported operations on the result entry.</param>
        /// <returns>The transformed entry of type <see cref="ResultInvitation" />.</returns>
        public ResultInvitation Transform(Invitation entry, IEnumerable<ILink> links)
        {
            return new ResultInvitation(
                entry.Id,
                entry.Roles,
                links);
        }
    }
}
