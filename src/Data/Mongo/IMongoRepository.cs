using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Core.Contracts;
using Core.Models;

namespace Data.Mongo
{
    public interface IMongoRepository<T> : INoSqlRepository<T> where T : BaseEntity
    {
        IQueryable<T> Filter(bool onlyEnabledEntities = true);

        Task Disabled(T entity);

        Task AddBatch(IEnumerable<T> entities);

        Task DeleteBatch(Expression<Func<T, bool>> filter);
    }
}