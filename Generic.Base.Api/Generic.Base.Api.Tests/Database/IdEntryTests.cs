namespace Generic.Base.Api.Tests.Database
{
    using Generic.Base.Api.Database;

    [Trait(
        "TestType",
        "UnitTest")]
    public class IdEntryTests
    {
        [Fact]
        public void Ctor()
        {
            var idEntry = new IdEntry(nameof(IdEntry));

            Assert.Equal(
                nameof(IdEntry),
                idEntry.Id);
        }
    }
}
