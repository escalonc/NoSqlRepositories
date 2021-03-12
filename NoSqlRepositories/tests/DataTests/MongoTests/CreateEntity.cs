using Core.Models;
using Data.Mongo;
using Xunit;

namespace DataTests.MongoTests
{
    public class CreateEntity
    {
        [Fact]
        public void Add_Inserts_Entity()
        {
            var mongoContext = new MongoContext(new DatabaseSettings
            {
                ConnectionString = "",
                DatabaseName = ""
            });
        }
    }
}