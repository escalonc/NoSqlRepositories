using MongoDB.Driver;
using NoSqlRepositories.Contracts;

namespace NoSqlRepositories.Mongo
{
    public interface IMongoContext
    {
        public IMongoClient Client { get; }

        public IAuditable Auditable { get; }

        IMongoCollection<T> GetCollection<T>(string name);
    }
}