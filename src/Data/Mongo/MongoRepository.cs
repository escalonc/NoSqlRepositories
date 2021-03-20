using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Ardalis.GuardClauses;
using Core.Contracts;
using Core.Models;
using MongoDB.Driver;

namespace Data.Mongo
{
    public class MongoRepository<T> : IMongoRepository<T> where T : BaseEntity
    {
        private readonly IAuditable _auditable;
        private readonly IMongoClient _client;
        private readonly IMongoCollection<T> _collection;

        protected MongoRepository(IMongoContext context, IAuditable auditable)
        {
            Guard.Against.Auditable(auditable, nameof(auditable));

            _auditable = auditable;
            _client = context.Client;
            _collection = context.GetCollection<T>(GetCollectionName(typeof(T)));
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

            entity.CreatedBy = _auditable.CreatedBy;
            entity.CreatedDate = _auditable.CreatedDate;

            await _collection.InsertOneAsync(entity);
        }

        public async Task Update(T entity)
        {
            Guard.Against.Null(entity, nameof(entity));

            entity.UpdatedBy = _auditable.UpdatedBy;
            entity.UpdatedDate = _auditable.UpdatedDate;

            await _collection.ReplaceOneAsync(e => e.Id == entity.Id, entity);
        }

        public async Task Delete(T entity)
        {
            Guard.Against.Null(entity, nameof(entity));
            await _collection.DeleteOneAsync(e => e.Id == entity.Id);
        }

        public IQueryable<T> Filter(bool onlyEnabledEntities = true)
        {
            return _collection
                .AsQueryable()
                .Where(e => e.Enabled == onlyEnabledEntities);
        }

        public async Task Disabled(T entity)
        {
            Guard.Against.Null(entity, nameof(entity));
            entity.Enabled = false;
            await Update(entity);
        }

        public async Task AddBatch(IEnumerable<T> entities)
        {
            Guard.Against.Null(entities, nameof(entities));
            
            using var session = await _client.StartSessionAsync();
            session.StartTransaction();
            try
            {
                await _collection.InsertManyAsync(entities);
            }
            catch
            {
                await session
                    .AbortTransactionAsync();
                throw;
            }

            await session.CommitTransactionAsync();
        }

        public async Task DeleteBatch(Expression<Func<T, bool>> filter)
        {
            using var session = await _client.StartSessionAsync();
            session.StartTransaction();
            try
            {
                await _collection.DeleteManyAsync(filter);
            }
            catch
            {
                await session
                    .AbortTransactionAsync();
                throw;
            }

            await session.CommitTransactionAsync();
        }

        private static string GetCollectionName(Type type)
        {
            Guard.Against.Null(type, nameof(type));

            var bsonCollectionAttribute =
                Attribute.GetCustomAttribute(type, typeof(BsonCollectionAttribute)) as BsonCollectionAttribute ??
                throw new InvalidOperationException(
                    $"Collection name must to be specified using: {nameof(BsonCollectionAttribute)}");

            return bsonCollectionAttribute.CollectionName;
        }
    }
}