using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BLRequests.DataLayer.BaseGenericRepository
{
    public interface IGenericRepository<TEntity> : IDisposable
        where TEntity : class
    {
        Task<TEntity?> CreateAsync(TEntity item);
        Task<TEntity?> FindByIdAsync(int id);
        Task<TEntity?> FindFirstAsync(Expression<Func<TEntity, bool>> filter);
        Task<TResult?> FindFirstAsync<TResult>(
            Expression<Func<TEntity, bool>> filter, 
            Expression<Func<TEntity, TResult>> select);
        Task<TEntity?> FindFirstAsync(
            Expression<Func<TEntity, bool>> filter,
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include);
        Task<TEntity?> GetCollection<TOut>(TEntity entity, Expression<Func<TEntity, IEnumerable<TOut>>> expression)
            where TOut : class;
        Task<TEntity?> GetCollectionWithInclude<TOut>(
            TEntity entity,
            Expression<Func<TEntity, IEnumerable<TOut>>> expression,
            Func<IQueryable<TOut>, IIncludableQueryable<TOut, object>> include) where TOut : class;
        Task<int> RemoveAsync(TEntity entity);
        Task<int> UpdateAsync(TEntity item);
        Task<int> UpdateAsync(Expression<Func<TEntity, bool>> filter,
            Expression<Func<SetPropertyCalls<TEntity>, SetPropertyCalls<TEntity>>> properties);
        Task<int> UpdateRangeAsync(IEnumerable<TEntity> items);
        Task<List<TEntity>?> GetAllAsync();
        Task<List<TEntity>?> GetAllAsync(
            Expression<Func<TEntity, bool>> filter,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,
            int skip = 0,
            int take = 50);
        Task<List<TResult>?> GetAllAsync<TResult>(
            Expression<Func<TEntity, TResult>> select,
            Expression<Func<TEntity, bool>>? filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,
            int skip = 0,
            int take = 50);
    }
}
