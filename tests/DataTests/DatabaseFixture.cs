using System;
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

            var context = new TestContext(databaseSettings);
            Repository = new TestRepository(context);
        }
        public void Dispose()
        {
            _runner.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}