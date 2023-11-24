namespace Generic.Base.Api.Tests.Models
{
    using Generic.Base.Api.Models;

    /// <summary>
    ///     Tests for <see cref="ErrorResult" />.
    /// </summary>
    [Trait(
        "TestType",
        "UnitTest")]
    public class ErrorResultTests
    {
        [Theory]
        [InlineData("the error")]
        public void Ctor(string error)
        {
            var errorResult = new ErrorResult(error);

            Assert.Equal(
                error,
                errorResult.Error);
        }
    }
}
