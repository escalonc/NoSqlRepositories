using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using NoSqlRepositories.Contracts;
using NoSqlRepositories.Models;

namespace NoSqlRepositories.Mongo
{
    public interface IMongoRepository<T> : INoSqlRepository<T> where T : BaseEntity
    {
        Task<IEnumerable<T>> Find(Expression<Func<T, bool>> filter);

        Task<IEnumerable<T>> Find(Expression<Func<T, bool>> filter, Expression<Func<T, object>> sortingField,
            SortOptions sortOptions = SortOptions.Ascending);

        Task<IEnumerable<TProjection>> Find<TProjection>(Expression<Func<T, bool>> filter,
            Expression<Func<T, TProjection>> projectionExpression, Expression<Func<T, object>> sortingField,
            SortOptions sortOptions = SortOptions.Ascending);

        Task Disable(T entity);

        Task DisableBatch(IList<T> entities);

        Task AddBatch(IList<T> entities);

        Task UpdateBatch(IList<T> entities);
        Task DeleteBatch(Expression<Func<T, bool>> filter);
    }
}