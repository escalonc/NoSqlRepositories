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

        protected MongoRepository(IMongoContext context)
        {
            _auditable = context.Auditable;
            _client = context.Client;
            _collection = context.GetCollection<T>(GetCollectionName(typeof(T)));
        }

        public async Task<IEnumerable<T>> FindAll(bool onlyEnabledEntities = true)
        {
            return await _collection
                .Find(FilterByEnabledEntities(onlyEnabledEntities))
                .ToListAsync();
        }

        public async Task<T> FindById(Guid id, bool onlyEnabledEntities = true)
        {
            Guard.Against.Null(id, nameof(id));

            // TODO: Review filter
            var filter = Builders<T>.Filter.Where(e => e.Id == id) & FilterByEnabledEntities(onlyEnabledEntities);

            return await _collection
                .Find(filter)
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

            // TODO: Support partial update

            await _collection.ReplaceOneAsync(e => e.Id == entity.Id, entity);
        }

        public async Task Delete(T entity)
        {
            Guard.Against.Null(entity, nameof(entity));
            await _collection.DeleteOneAsync(e => e.Id == entity.Id);
        }

        public async Task<long> Count(bool onlyEnabledEntities = true)
        {
            return await _collection.CountDocumentsAsync(FilterByEnabledEntities(onlyEnabledEntities));
        }

        public async Task<IEnumerable<T>> Find(Expression<Func<T, bool>> filter)
        {
            return await _collection.Find(filter).ToListAsync();
        }

        public Task<IEnumerable<T>> Find(Expression<Func<T, bool>> filter, Expression<Func<T, object>> sortingField,
            SortOptions sortOptions = SortOptions.Ascending)
        {
            return Sort(_collection.Find(filter), sortingField, sortOptions);
        }

        public async Task<IEnumerable<TProjection>> Find<TProjection>(Expression<Func<T, bool>> filter,
            Expression<Func<T, TProjection>> projectionExpression, Expression<Func<T, object>> sortingField,
            SortOptions sortOptions = SortOptions.Ascending)
        {
            return await Sort(_collection.Find(filter).Project(projectionExpression), sortingField, sortOptions);
        }

        private async Task<IEnumerable<TResult>> Sort<TResult>(IFindFluent<T, TResult> collection,
            Expression<Func<T, object>> sortingField, SortOptions sortOptions)
        {
            var data = collection.SortBy(sortingField);

            if (sortOptions == SortOptions.Descending)
            {
                data = collection.SortByDescending(sortingField);
            }

            return await data.ToListAsync();
        }

        public async Task Disable(T entity)
        {
            Guard.Against.Null(entity, nameof(entity));
            entity.Enabled = false;
            await Update(entity);
        }

        public async Task DisableBatch(IList<T> entities)
        {
            using var session = await _client.StartSessionAsync();
            session.StartTransaction();
            try
            {
                var ids = entities.Select(x => x.Id);
                var updateDefinition = Builders<T>.Update.Set(x => x.Enabled, false);
                await _collection.UpdateManyAsync(x => ids.Contains(x.Id), updateDefinition);
            }
            catch
            {
                await session
                    .AbortTransactionAsync();
                throw;
            }

            await session.CommitTransactionAsync();
        }

        public async Task AddBatch(IList<T> entities)
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

        public async Task UpdateBatch(IList<T> entities)
        {
            using var session = await _client.StartSessionAsync();
            session.StartTransaction();
            try
            {
                foreach (var entity in entities)
                {
                    await _collection.ReplaceOneAsync(x => x.Id == entity.Id, entity);
                }
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

        private static FilterDefinition<T> FilterByEnabledEntities(bool onlyEnabledEntities)
        {
            var filter = Builders<T>.Filter.Where(x => x.Enabled);

            if (!onlyEnabledEntities)
            {
                filter = FilterDefinition<T>.Empty;
            }

            return filter;
        }
    }
}