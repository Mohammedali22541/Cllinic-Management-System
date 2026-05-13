using ClinicApp.Data.Entites.Common;

namespace ClinicApp.Data.DatabaseContract
{
    public interface IUnitOfWork
    {
        Task<int> SaveChangesAsync();

        IGenericRepository<TEntity, TKey> GetRepository<TEntity, TKey>() where TEntity : BaseEntity<TKey>;
    }
}