using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NoSqlRepositories.Models;

namespace NoSqlRepositories.Contracts
{
    public interface INoSqlRepository<T> where T : BaseEntity
    {
        Task<IEnumerable<T>> FindAll(bool onlyEnabledEntities = true);

        Task<T> FindById(Guid id, bool onlyEnabledEntities = true);

        Task Add(T entity);

        Task Update(T entity);

        Task Delete(T entity);

        Task<long> Count(bool onlyEnabledEntities = true);
    }
}