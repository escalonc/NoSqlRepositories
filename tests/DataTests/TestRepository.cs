using Data.Mongo;

namespace DataTests
{
    public class TestRepository : MongoRepository<TestEntity>
    {
        public TestRepository(IMongoContext context) : base(context)
        {
        }
    }
}