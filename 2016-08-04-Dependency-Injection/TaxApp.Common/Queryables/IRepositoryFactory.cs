using EntityFramework.Repository;
using System.Data.Entity;

namespace TaxApp.Common.Queryables
{
    public interface IRepositoryFactory<TContext>
        where TContext : IDbContext
    {
        IRepository<TContext, T> GetRepository<T>() where T : class;
        IDbSet<T> GetSet<T>() where T : class;
    }
}