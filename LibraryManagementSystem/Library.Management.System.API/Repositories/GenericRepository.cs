using Library.Management.System.API.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Library.Management.System.API.Repositories
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class
    {
        private readonly LibraryContext _context;

        public GenericRepository(LibraryContext context) 
            => _context = context;
        
        /// <summary>
        /// Get a specific entity by id
        /// </summary>
        /// <param name="id">Used to retrieve entity</param>
        /// <returns>the matched entity</returns>
        public async Task<TEntity?> GetByIdAsync(int id)
            => await _context.Set<TEntity>().FindAsync(id);

        /// <summary>
        /// Get all entities of type TEntity
        /// </summary>
        /// <returns>Retrieve all entities</returns>
        public async Task<IReadOnlyList<TEntity>> GetAllAsync()
            => await _context.Set<TEntity>().ToListAsync();

        /// <summary>
        /// Add an entity to the database
        /// </summary>
        /// <param name="entity"></param>
        public void Add(TEntity entity)
            => _context.Set<TEntity>().Add(entity);

        /// <summary>
        /// Update an entity in the database
        /// </summary>
        /// <param name="entity"></param>
        public void Update(TEntity entity)
            => _context.Set<TEntity>().Update(entity);

        /// <summary>
        /// Delete an entity from the database
        /// </summary>
        /// <param name="entity"></param>
        public void Delete(TEntity entity)
            => _context.Set<TEntity>().Remove(entity);

        /// <summary>
        /// Find entities that match the predicate
        /// </summary>
        /// <param name="predicate">  </param>
        /// <returns></returns>
        public async Task<IReadOnlyList<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate)
            => await _context.Set<TEntity>().Where(predicate).ToListAsync();

        /// <summary>
        /// Find entities that match the predicate
        /// </summary>
        /// <param name="predicate">  </param>
        /// <returns></returns>
        public IQueryable<TEntity> Find(Expression<Func<TEntity, bool>> predicate)
        {
            return _context.Set<TEntity>().Where(predicate);
        }



        public async Task<IReadOnlyList<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate, int page, int pageSize)
        {
            return await _context.Set<TEntity>().Where(predicate).Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();
        }
    }
}
