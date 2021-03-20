using System;
using Core.Contracts;
using Core.Models;
using Data.Mongo;
using Mongo2Go;

namespace DataTests
{
    public class DatabaseFixture : IDisposable
    {
        private static MongoDbRunner _runner;
        protected readonly IMongoRepository<TestDocument> Repository;

        protected DatabaseFixture()
        {
            _runner = MongoDbRunner.Start(singleNodeReplSet: true);
            var databaseSettings = new DatabaseSettings
            {
                ConnectionString = _runner.ConnectionString,
                DatabaseName = "tests"
            };

            IAuditable auditable = new BaseEntity
                {CreatedBy = "Chris", UpdatedBy = "Chris", CreatedDate = DateTime.Now, UpdatedDate = DateTime.Now};

            var context = new TestContext(databaseSettings);
            Repository = new TestRepository(context, auditable);
        }

        public void Dispose()
        {
            _runner.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}