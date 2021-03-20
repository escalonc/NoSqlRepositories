using Core.Contracts;
using Core.Models;
using Data.Mongo;

namespace DataTests
{
    public class TestRepository : MongoRepository<TestDocument>
    {
        public TestRepository(IMongoContext context, IAuditable auditable) : base(context, auditable)
        {
        }
    }
}