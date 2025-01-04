using Library.Management.System.API.Models;
using Library.Management.System.API.Repositories;
using System.Collections;

namespace Library.Management.System.API.Unit_of_Work
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly LibraryContext _context;
        private Hashtable _repositories;

        public UnitOfWork(LibraryContext context)
        {
            _context = context;
        }

        public IGenericRepository<TEntity> Repository<TEntity>() where TEntity : class
        {
            _repositories ??= new Hashtable();

            var type = typeof(TEntity).Name;
            if (!_repositories.ContainsKey(type))
            {
                var repository = new GenericRepository<TEntity>(_context);
                _repositories.Add(type, repository);
            }

            return (IGenericRepository<TEntity>)_repositories[type]!;
        }

        public async Task<int> SaveChangesAsync() => await _context.SaveChangesAsync();

        public void Dispose() => _context.Dispose();
    }
}
