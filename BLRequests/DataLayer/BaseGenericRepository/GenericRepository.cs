using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System.Collections;
using System.Linq.Expressions;

namespace BLRequests.DataLayer.BaseGenericRepository
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity>
        where TEntity : class
    {
        DbContext _context;
        DbSet<TEntity> _dbSet;

        public GenericRepository(DbContext db)
        {
            _context = db;
            _dbSet = _context.Set<TEntity>();
        }
        public async Task<TEntity?> CreateAsync(TEntity entity)
        {
            try
            {
                if (entity == null)
                    return null;

                if (entity is IEnumerable)
                    await _dbSet.AddRangeAsync(entity);
                else
                    await _dbSet.AddAsync(entity);

                await _context.SaveChangesAsync();
                return entity;
            }
            catch (ArgumentNullException ex)
            {
                Console.WriteLine($"Ничего не найдено: {ex.Message}");
            }
            catch (OperationCanceledException ex)
            {
                Console.WriteLine($"Отмена операции: {ex.Message}");
            }
            return null;
        }

        public async Task<TEntity?> FindByIdAsync(int id)
        {
            try
            {
                return await _dbSet.FindAsync(id);
            }
            catch (ArgumentNullException ex)
            {
                Console.WriteLine($"Ничего не найдено: {ex.Message}");
            }
            catch (OperationCanceledException ex)
            {
                Console.WriteLine($"Отмена операции: {ex.Message}");
            }
            return null;
        }

        public async Task<TEntity?> FindFirstAsync(Expression<Func<TEntity, bool>> filter)
        {
            try
            {
                TEntity? _result = await _dbSet.Where(filter).FirstOrDefaultAsync();

                return _result;
            }
            catch (ArgumentNullException ex)
            {
                Console.WriteLine($"Ничего не найдено: {ex.Message}");
            }
            catch (OperationCanceledException ex)
            {
                Console.WriteLine($"Отмена операции: {ex.Message}");
            }
            return default;
        }

        public async Task<TResult?> FindFirstAsync<TResult>(
            Expression<Func<TEntity, bool>> filter, 
            Expression<Func<TEntity, TResult>> select)
        {
            try
            {
                IQueryable<TEntity>? _resetSet = _dbSet.Where(filter).AsQueryable();
                TResult? _result = await _resetSet.Select(select).FirstOrDefaultAsync();
                
                return _result;
            }
            catch (ArgumentNullException ex)
            {
                Console.WriteLine($"Ничего не найдено: {ex.Message}");
            }
            catch (OperationCanceledException ex)
            {
                Console.WriteLine($"Отмена операции: {ex.Message}");
            }
            return default;
        }

        public async Task<TEntity?> FindFirstAsync(
            Expression<Func<TEntity, bool>> filter,
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include)
        {
            try
            {
                IQueryable<TEntity>? _resetSet = _dbSet.Where(filter).AsQueryable();
                _resetSet = include(_resetSet);
                TEntity? _result = await _resetSet.FirstOrDefaultAsync();

                return _result;
            }
            catch (ArgumentNullException ex)
            {
                Console.WriteLine($"Ничего не найдено: {ex.Message}");
            }
            catch (OperationCanceledException ex)
            {
                Console.WriteLine($"Отмена операции: {ex.Message}");
            }
            return default;
        }

        public async Task<TResult?> FindFirstAsync<TResult>(
            Expression<Func<TEntity, bool>> filter,
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include,
            Expression<Func<TEntity, TResult>> select)
        {
            try
            {
                IQueryable<TEntity>? _resetSet = _dbSet.Where(filter).AsQueryable();
                _resetSet = include(_resetSet);
                TResult? _result = await _resetSet.Select(select).FirstOrDefaultAsync();

                return _result;
            }
            catch (ArgumentNullException ex)
            {
                Console.WriteLine($"Ничего не найдено: {ex.Message}");
            }
            catch (OperationCanceledException ex)
            {
                Console.WriteLine($"Отмена операции: {ex.Message}");
            }
            return default;
        }

        public async Task<List<TEntity>?> GetAllAsync()
        {
            try
            {
                IQueryable<TEntity>? _resetSet = _dbSet.AsNoTracking().AsQueryable();
                return await _resetSet.ToListAsync();
            }
            catch (ArgumentNullException ex)
            {
                Console.WriteLine($"Ничего не найдено: {ex.Message}");
            }
            catch (OperationCanceledException ex)
            {
                Console.WriteLine($"Отмена операции: {ex.Message}");
            }
            return null;
        }

        public async Task<List<TEntity>?> GetAllAsync(
            Expression<Func<TEntity, bool>> filter, 
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,
            int skip = 0, 
            int take = 50)
        {
            try
            {
                IQueryable<TEntity>? _resetSet = _dbSet.AsNoTracking().Where(filter).AsQueryable();

                if (include != null)
                    _resetSet = include(_resetSet);

                if (orderBy != null)
                    _resetSet = orderBy(_resetSet);

                if (take != 0)
                    _resetSet = skip == 0 ? _resetSet.Take(take) : _resetSet.Skip(skip).Take(take);

                return await _resetSet.ToListAsync();
            }
            catch (ArgumentNullException ex)
            {
                Console.WriteLine($"Ничего не найдено: {ex.Message}");
            }
            catch (OperationCanceledException ex)
            {
                Console.WriteLine($"Отмена операции: {ex.Message}");
            }
            return null;
        }

        public async Task<List<TResult>?> GetAllAsync<TResult>(
            Expression<Func<TEntity, TResult>> select,
            Expression<Func<TEntity, bool>>? filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,
            int skip = 0,
            int take = 50)
        {
            try
            {
                IQueryable<TEntity>? _resetSet = filter != null ? 
                    _dbSet.AsNoTracking().Where(filter).AsQueryable() : 
                    _dbSet.AsNoTracking().AsQueryable();

                if (include != null)
                    _resetSet = include(_resetSet);

                if (orderBy != null)
                    _resetSet = orderBy(_resetSet);

                if (take != 0)
                    _resetSet = skip == 0 ? _resetSet.Take(take) : _resetSet.Skip(skip).Take(take);

                IQueryable<TResult> _result = _resetSet.Select(select).AsQueryable();

                return await _result.ToListAsync();
            }
            catch (ArgumentNullException ex)
            {
                Console.WriteLine($"Ничего не найдено: {ex.Message}");
            }
            catch (OperationCanceledException ex)
            {
                Console.WriteLine($"Отмена операции: {ex.Message}");
            }
            return null;
        }

        public async Task<TEntity?> GetCollection<TOut>(TEntity entity, Expression<Func<TEntity, IEnumerable<TOut>>> expression)
            where TOut : class
        {
            try
            {
                await _context.Entry(entity).Collection(expression).LoadAsync();
                return entity;
            }
            catch (ArgumentNullException ex)
            {
                Console.WriteLine($"Ничего не найдено: {ex.Message}");
            }
            catch (OperationCanceledException ex)
            {
                Console.WriteLine($"Отмена операции: {ex.Message}");
            }
            return null;
        }

        public async Task<TEntity?> GetCollectionWithInclude<TOut>(
            TEntity entity, 
            Expression<Func<TEntity, IEnumerable<TOut>>> expression,
            Func<IQueryable<TOut>, IIncludableQueryable<TOut, object>> include) where TOut : class
        {
            try
            {
                await include(_context.Entry(entity).Collection(expression).Query()).LoadAsync();
                return entity;
            }
            catch (ArgumentNullException ex)
            {
                Console.WriteLine($"Ничего не найдено: {ex.Message}");
            }
            catch (OperationCanceledException ex)
            {
                Console.WriteLine($"Отмена операции: {ex.Message}");
            }
            return null;
        }

        public async Task<int> RemoveAsync(TEntity entity)
        {
            try
            {
                _context.Entry(entity).State = EntityState.Deleted;
                return await _context.SaveChangesAsync();
            }
            catch (ArgumentNullException ex)
            {
                Console.WriteLine($"Ничего не найдено: {ex.Message}");
            }
            catch (OperationCanceledException ex)
            {
                Console.WriteLine($"Отмена операции: {ex.Message}");
            }
            return 0;
        }

        public async Task<int> UpdateAsync(TEntity entity)
        {
            try
            {
                _dbSet.Update(entity);
                return await _context.SaveChangesAsync();
            }
            catch (ArgumentNullException ex)
            {
                Console.WriteLine($"Ничего не найдено: {ex.Message}");
            }
            catch (OperationCanceledException ex)
            {
                Console.WriteLine($"Отмена операции: {ex.Message}");
            }
            return 0;
        }

        public async Task<int> UpdateRangeAsync(IEnumerable<TEntity> items)
        {
            try
            {
                _dbSet.UpdateRange(items);
                return await _context.SaveChangesAsync();
            }
            catch (ArgumentNullException ex)
            {
                Console.WriteLine($"Ничего не найдено: {ex.Message}");
            }
            catch (OperationCanceledException ex)
            {
                Console.WriteLine($"Отмена операции: {ex.Message}");
            }
            return 0;
        }

        public async Task<int> UpdateAsync(Expression<Func<TEntity, bool>> filter,
            Expression<Func<SetPropertyCalls<TEntity>, SetPropertyCalls<TEntity>>> properties)
        {
            try
            {
                return await _dbSet.Where(filter).ExecuteUpdateAsync(properties);
            }
            catch (ArgumentNullException ex)
            {
                Console.WriteLine($"Ничего не найдено: {ex.Message}");
            }
            catch (OperationCanceledException ex)
            {
                Console.WriteLine($"Отмена операции: {ex.Message}");
            }
            return 0;
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
