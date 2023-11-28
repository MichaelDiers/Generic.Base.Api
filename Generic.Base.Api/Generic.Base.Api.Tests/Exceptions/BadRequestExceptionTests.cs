namespace Generic.Base.Api.Tests.Exceptions
{
    using Generic.Base.Api.Exceptions;

    /// <summary>
    ///     Tests for <see cref="BadRequestException" />.
    /// </summary>
    [Trait(
        "TestType",
        "UnitTest")]
    public class BadRequestExceptionTests
    {
        [Fact]
        public void EmptyCtor()
        {
            var message = $"Exception of type '{typeof(BadRequestException).FullName}' was thrown.";
            var exception = new BadRequestException();

            Assert.IsAssignableFrom<Exception>(exception);
            Assert.Equal(
                message,
                exception.Message);
            Assert.Null(exception.InnerException);
        }

        [Fact]
        public void MessageCtor()
        {
            const string message = nameof(this.MessageCtor);

            var exception = new BadRequestException(message);

            Assert.IsAssignableFrom<Exception>(exception);
            Assert.Equal(
                message,
                exception.Message);
            Assert.Null(exception.InnerException);
        }

        [Fact]
        public void MessageInnerExceptionCtor()
        {
            const string message = nameof(this.MessageCtor);
            var innerException = new Exception();

            var exception = new BadRequestException(
                message,
                innerException);

            Assert.IsAssignableFrom<Exception>(exception);
            Assert.Equal(
                message,
                exception.Message);
            Assert.Equal(
                innerException,
                exception.InnerException);
        }
    }
}
