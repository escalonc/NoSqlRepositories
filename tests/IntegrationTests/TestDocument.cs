using NoSqlRepositories.Models;
using NoSqlRepositories.Mongo;

namespace IntegrationTests
{
    [BsonCollection("things")]
    public class TestDocument : BaseEntity
    {
        public string? Name { get; set; }
    }
}