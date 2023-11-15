namespace Generic.Base.Api.Tests.Exceptions
{
    using Generic.Base.Api.Exceptions;

    /// <summary>
    ///     Tests for <see cref="NotFoundException" />.
    /// </summary>
    public class NotFoundExceptionTests
    {
        [Fact]
        public void EmptyCtor()
        {
            var message = $"Exception of type '{typeof(NotFoundException).FullName}' was thrown.";
            var exception = new NotFoundException();

            Assert.IsAssignableFrom<Exception>(exception);
            Assert.Equal(
                message,
                exception.Message);
            Assert.Null(exception.InnerException);
        }

        [Fact]
        public void MessageCtor()
        {
            var message = nameof(this.MessageCtor);

            var exception = new NotFoundException(message);

            Assert.IsAssignableFrom<Exception>(exception);
            Assert.Equal(
                message,
                exception.Message);
            Assert.Null(exception.InnerException);
        }

        [Fact]
        public void MessageInnerExceptionCtor()
        {
            var message = nameof(this.MessageCtor);
            var innerException = new Exception();

            var exception = new NotFoundException(
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
