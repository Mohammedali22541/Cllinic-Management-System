using ClinicApp.Data.Context;
using ClinicApp.Data.DatabaseContract;
using ClinicApp.Data.Entites.Common;

namespace ClinicApp.Data.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ClinicManagementDbContext _dbContext;
        private readonly Dictionary<Type, object> _repositories = new Dictionary<Type, object>();

        public UnitOfWork(ClinicManagementDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IGenericRepository<TEntity, TKey> GetRepository<TEntity, TKey>() where TEntity : BaseEntity<TKey>
        {
            var type = typeof(TEntity);

            if (_repositories.TryGetValue(type, out var repository))
                return (IGenericRepository<TEntity, TKey>)repository;

            var newRepository = new GenericRepository<TEntity, TKey>(_dbContext);

            _repositories[type] = newRepository;

            return newRepository;
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _dbContext.SaveChangesAsync();
        }
    }
}