using System;
using Core.Contracts;
using Core.Models;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.IdGenerators;
using MongoDB.Driver;

namespace Data.Mongo
{
    public class MongoContext : IMongoContext
    {
        private readonly IMongoDatabase _mongoDatabase;

        public MongoContext(IDatabaseSettings settings)
        {
            Client = new MongoClient(settings.ConnectionString);
            _mongoDatabase = Client.GetDatabase(settings.DatabaseName);

            ModelBuilder();
        }

        public IMongoCollection<T> GetCollection<T>(string collectionName)
        {
            return _mongoDatabase.GetCollection<T>(collectionName);
        }

        public IMongoClient Client { get; }

        private static void ModelBuilder()
        {
            BsonClassMap.RegisterClassMap<BaseEntity>(cm =>
            {
                cm.AutoMap();
                cm.MapIdField(c => c.Id).SetIdGenerator(CombGuidGenerator.Instance);
            });
        }
    }
}