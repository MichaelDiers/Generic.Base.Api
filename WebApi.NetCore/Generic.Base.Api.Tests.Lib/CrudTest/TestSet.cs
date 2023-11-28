﻿namespace Generic.Base.Api.Tests.Lib.CrudTest
{
    public class TestSet<TCreate> where TCreate : class
    {
        public TestSet(TestData<TCreate> create)
        {
            this.Create = create;
        }

        public TestData<TCreate> Create { get; }
    }
}