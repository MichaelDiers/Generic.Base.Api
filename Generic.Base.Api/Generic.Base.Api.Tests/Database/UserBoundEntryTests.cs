namespace Generic.Base.Api.Tests.Database
{
    using Generic.Base.Api.Database;

    /// <summary>
    ///     Tests for <see cref="UserBoundEntry" />.
    /// </summary>
    [Trait(
        "TestType",
        "UnitTest")]
    public class UserBoundEntryTests
    {
        [Theory]
        [InlineData(
            "id",
            "userId")]
        public void Ctor(string id, string userId)
        {
            var entry = new UserBoundEntry(
                id,
                userId);

            Assert.Equal(
                id,
                entry.Id);
            Assert.Equal(
                userId,
                entry.UserId);
        }

        [Theory]
        [InlineData(
            "id",
            "userId")]
        public void IsAssignableFromIUserBoundEntry(string id, string userId)
        {
            var entry = new UserBoundEntry(
                id,
                userId);

            Assert.IsAssignableFrom<IUserBoundEntry>(entry);
        }
    }
}
