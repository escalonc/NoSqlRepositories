using System.Threading.Tasks;
using Core.Models;
using Data.Mongo;
using Xunit;

namespace DataTests.MongoTests
{
    public class CreateEntity
    {
        [Fact]
        public async Task Add_Inserts_Entity()
        {
            var mongoContext = new MongoContext(new DatabaseSettings
            {
                ConnectionString = "mongodb+srv://escalonc:admin#123#@cluster0.ywtwn.mongodb.net/?retryWrites=true&w=majority",
                DatabaseName = "tests"
            });

            var testRepository = new TestRepository(mongoContext);

            await testRepository.Add(new TestEntity { Name = "Hello"});

        }
    }
}