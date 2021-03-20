using Ardalis.GuardClauses;
using Core.Contracts;
using Core.Models;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.IdGenerators;
using MongoDB.Driver;

namespace Data.Mongo
{
    public class MongoContext : IMongoContext
    {
        private static bool _isModelSetup;
        private readonly IMongoDatabase _mongoDatabase;

        protected MongoContext(IDatabaseSettings settings, IAuditable auditable)
        {
            Guard.Against.Auditable(auditable, nameof(auditable));

            Auditable = auditable;
            Client = new MongoClient(settings.ConnectionString);
            _mongoDatabase = Client.GetDatabase(settings.DatabaseName);

            Initialize();
        }

        public IMongoCollection<T> GetCollection<T>(string collectionName)
        {
            return _mongoDatabase.GetCollection<T>(collectionName);
        }

        public IMongoClient Client { get; }

        public IAuditable Auditable { get; }

        private void Initialize()
        {
            if (_isModelSetup) return;
            OnModelCreating();
            _isModelSetup = true;
        }

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