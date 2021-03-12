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
        private static bool _isModelSetup;

        protected MongoContext(IDatabaseSettings settings)
        {
            Client = new MongoClient(settings.ConnectionString);
            _mongoDatabase = Client.GetDatabase(settings.DatabaseName);

            Initialize();
        }

        private void Initialize()
        {
            if (_isModelSetup) return;
            OnModelCreating();
            _isModelSetup = true;
        }

        public IMongoCollection<T> GetCollection<T>(string collectionName)
        {
            return _mongoDatabase.GetCollection<T>(collectionName);
        }

        public IMongoClient Client { get; }

        protected virtual void OnModelCreating()
        {
            BsonClassMap.RegisterClassMap<BaseEntity>(cm =>
            {
                cm.AutoMap();
                cm.MapIdField(c => c.Id).SetIdGenerator(CombGuidGenerator.Instance);
            });
        }
    }
}