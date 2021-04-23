using NoSqlRepositories.Mongo;

namespace IntegrationTests
{
    public class TestRepository : MongoRepository<TestDocument>
    {
        public TestRepository(IMongoContext context) : base(context)
        {
        }
    }
}