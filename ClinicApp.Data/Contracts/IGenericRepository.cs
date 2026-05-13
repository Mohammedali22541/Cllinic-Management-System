using ClinicApp.Data.Entites.Common;
using System.Linq.Expressions;

namespace ClinicApp.Data.DatabaseContract
{
    public interface IGenericRepository<TEntity, TKey> where TEntity : BaseEntity<TKey>
    {
        Task<IEnumerable<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>>? filter = null, List<Expression<Func<TEntity, object>>>? includes = null);

        Task<TEntity?> GetByIdAsync(TKey id);

        Task AddAsync(TEntity entity);

        void Update(TEntity entity);

        void Delete(TEntity entity);
    }
}