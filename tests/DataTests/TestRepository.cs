using Data.Mongo;

namespace DataTests
{
    public class TestRepository : MongoRepository<TestDocument>
    {
        public TestRepository(IMongoContext context) : base(context)
        {
        }
    }
}