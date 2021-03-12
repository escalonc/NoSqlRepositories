using MongoDB.Driver;

namespace Data.Mongo
{
    public interface IMongoContext
    {
        IMongoCollection<T> GetCollection<T>(string name);
    }
}