namespace Generic.Base.Api.Test.Lib.CrudTest
{
    using System.Net;
    using Generic.Base.Api.AuthServices.UserService;

    public class TestData<T> where T : class
    {
        public TestData(
            T data,
            string urnNamespace,
            HttpStatusCode expectedStatusCode,
            Role[] roles
        )
        {
            this.Data = data;
            this.UrnNamespace = urnNamespace;
            this.ExpectedStatusCode = expectedStatusCode;
            this.Roles = roles;
        }

        public T Data { get; }
        public HttpStatusCode ExpectedStatusCode { get; }
        public Role[] Roles { get; }
        public string UrnNamespace { get; }
    }
}
