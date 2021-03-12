using Core.Contracts;
using Data.Mongo;
using JetBrains.Annotations;

namespace DataTests
{
    public class TestContext : MongoContext
    {
        public TestContext([NotNull] IDatabaseSettings settings) : base(settings)
        {
        }
    }
}