namespace WebApi.NetCore.Api.Models
{
    using Generic.Base.Api.Database;
    using MongoDB.Bson.Serialization.Attributes;

    public class DatabaseEntry : IIdEntry
    {
        /// <summary>
        ///     Gets the id of the entry that is used in the database.
        /// </summary>
        [BsonId]
        public string? Id { get; set; }
    }
}
