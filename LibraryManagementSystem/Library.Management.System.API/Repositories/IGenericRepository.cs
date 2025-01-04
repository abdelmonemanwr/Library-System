using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Library.Management.System.API.Repositories
{
    public interface IGenericRepository<TEntity> where TEntity : class
    {
        void Add(TEntity entity);
        
        void Update(TEntity entity);
        
        void Delete(TEntity entity);

        Task<TEntity?> GetByIdAsync(int id);
        
        Task<IReadOnlyList<TEntity>> GetAllAsync();

        IQueryable<TEntity> Find(Expression<Func<TEntity, bool>> predicate);

        Task<IReadOnlyList<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate);

        Task<IReadOnlyList<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate, int page, int pageSize);  
    }
}
