using Core.Contracts;
using MongoDB.Driver;

namespace Data.Mongo
{
    public interface IMongoContext
    {
        public IMongoClient Client { get; }

        public IAuditable Auditable { get; }

        IMongoCollection<T> GetCollection<T>(string name);
    }
}