using ClinicApp.Data.Context;
using ClinicApp.Data.DatabaseContract;
using ClinicApp.Data.Entites.Common;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace ClinicApp.Data.Repositories
{
    public class GenericRepository<TEntity, TKey> : IGenericRepository<TEntity, TKey> where TEntity : BaseEntity<TKey>
    {
        private readonly ClinicManagementDbContext _dbContext;

        public GenericRepository(ClinicManagementDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>>? filter = null, List<Expression<Func<TEntity, object>>>? includes = null)
        {
            var query = _dbContext.Set<TEntity>().AsQueryable();

            if (filter is not null)
            {
                query = query.Where(filter);
            }

            if (includes is not null)
            {
                foreach (var include in includes)
                {
                    query = query.Include(include);
                }
            }
            return await query.ToListAsync();
        }

        public async Task AddAsync(TEntity entity)
        {
            await _dbContext.Set<TEntity>().AddAsync(entity);
        }

        public void Delete(TEntity entity)
        {
            _dbContext.Set<TEntity>().Remove(entity);
        }

        public async Task<TEntity?> GetByIdAsync(TKey id)
        {
            return await _dbContext.Set<TEntity>().FindAsync(id);
        }

        public void Update(TEntity entity)
        {
            _dbContext.Set<TEntity>().Update(entity);
        }
    }
}