namespace Generic.Base.Api.Tests.Extensions
{
    /// <summary>
    ///     Dummy configuration entry.
    /// </summary>
    internal class ExtensionTest
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="ExtensionTest" /> class.
        /// </summary>
        /// <param name="extensionTestKey1">The extension test key1.</param>
        /// <param name="extensionTestKey2">The extension test key2.</param>
        public ExtensionTest(string extensionTestKey1, string extensionTestKey2)
        {
            this.ExtensionTestKey1 = extensionTestKey1;
            this.ExtensionTestKey2 = extensionTestKey2;
        }

        /// <summary>
        ///     Gets the extension test key1.
        /// </summary>
        /// <value>
        ///     The extension test key1.
        /// </value>
        public string ExtensionTestKey1 { get; }

        /// <summary>
        ///     Gets the extension test key2.
        /// </summary>
        /// <value>
        ///     The extension test key2.
        /// </value>
        public string ExtensionTestKey2 { get; }
    }
}
