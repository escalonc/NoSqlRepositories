using MongoDB.Driver;

namespace Data.Mongo
{
    public interface IMongoContext
    {
        public IMongoClient Client { get; }
        IMongoCollection<T> GetCollection<T>(string name);
    }
}