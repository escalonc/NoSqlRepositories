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
        IQueryable<T> Filter(bool onlyEnabledEntities);

        Task BatchInsert(IEnumerable<T> entities);
        
        Task BatchDelete(Expression<Func<T, bool>> filter);
    }
}