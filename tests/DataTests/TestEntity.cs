using Core.Models;
using Data.Mongo;

namespace DataTests
{
    [BsonCollection("things")]
    public class TestEntity : BaseEntity
    {
        public string Name { get; set; }
    }
}