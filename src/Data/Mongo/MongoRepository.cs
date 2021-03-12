using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ardalis.GuardClauses;
using Core.Models;
using MongoDB.Driver;

namespace Data.Mongo
{
    public class MongoRepository<T> : IMongoRepository<T> where T : BaseEntity
    {
        private readonly IMongoCollection<T> _collection;

        public MongoRepository(IMongoContext context)
        {
            _collection = context.GetCollection<T>(GetCollectionName(typeof(T)));
        }

        private static string GetCollectionName(Type type)
        {
            var bsonCollectionAttribute =
                (BsonCollectionAttribute) Attribute.GetCustomAttribute(type, typeof(BsonCollectionAttribute));

            if (bsonCollectionAttribute == null)
            {
                throw new InvalidOperationException(
                    $"Collection name must to be specified using: {nameof(BsonCollectionAttribute)}");
            }

            return bsonCollectionAttribute.CollectionName;
        }

        public async Task<IEnumerable<T>> FindAll(bool onlyEnabledEntities = true)
        {
            return await _collection
                .Find(e => e.Enabled == onlyEnabledEntities)
                .ToListAsync();
        }

        public async Task<T> FindById(Guid id, bool onlyEnabledEntities = true)
        {
            Guard.Against.Null(id, nameof(id));
            return await _collection
                .Find(e => e.Id == id && e.Enabled == onlyEnabledEntities)
                .FirstOrDefaultAsync();
        }

        public async Task Add(T entity)
        {
            Guard.Against.Null(entity, nameof(entity));
            await _collection.InsertOneAsync(entity);
        }

        public async Task Update(T entity)
        {
            Guard.Against.Null(entity, nameof(entity));
            await _collection.ReplaceOneAsync(e => e.Id == entity.Id, entity);
        }

        public async Task Delete(T entity)
        {
            Guard.Against.Null(entity, nameof(entity));
            await _collection.DeleteOneAsync(e => e.Id == entity.Id);
        }

        public IQueryable Filter(bool onlyEnabledEntities = true)
        {
            return _collection
                .AsQueryable()
                .Where(e => e.Enabled == onlyEnabledEntities);
        }
    }
}