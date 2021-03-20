using Core.Models;
using Data.Mongo;

namespace DataTests
{
    [BsonCollection("things")]
    public class TestDocument : BaseEntity
    {
        public string? Name { get; set; }
    }
}