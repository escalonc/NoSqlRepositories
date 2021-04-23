using System;
using NoSqlRepositories.Contracts;
using NoSqlRepositories.Models;
using NoSqlRepositories.Mongo;

namespace IntegrationTests
{
    public class TestContext : MongoContext
    {
        public TestContext(IDatabaseSettings settings) : base(settings,
            new BaseEntity
                {CreatedBy = "Chris", CreatedDate = DateTime.Now, UpdatedBy = "Chris", UpdatedDate = DateTime.Now})
        {
        }
    }
}