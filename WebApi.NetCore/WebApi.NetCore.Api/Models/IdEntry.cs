namespace WebApi.NetCore.Api.Models
{
    using System.Text.Json.Serialization;
    using Generic.Base.Api.Database;

    public class IdEntry : IIdEntry
    {
        protected IdEntry(string id)
        {
            this.Id = id;
        }

        /// <summary>
        ///     Gets the identifier.
        /// </summary>
        [JsonIgnore]
        public string Id { get; set; }
    }
}
