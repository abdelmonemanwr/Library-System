using Library.Management.System.API.Models;
using Library.Management.System.API.Repositories;

namespace Library.Management.System.API.Unit_of_Work
{
    public interface IUnitOfWork: IDisposable
    {
        IGenericRepository<TEntity> Repository<TEntity>() where TEntity : class;
        Task<int> SaveChangesAsync();
    }
}
