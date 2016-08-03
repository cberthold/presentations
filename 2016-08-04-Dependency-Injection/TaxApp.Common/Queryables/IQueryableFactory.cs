using EntityFramework.Repository;
using System.Linq;

namespace TaxApp.Common.Queryables
{
    public interface IQueryableFactory<TContext>
        where TContext : IDbContext
    {
        IRepository<TContext, T> GetRepository<T>() where T : class;
        IQueryable<T> GetQueryableNoTracking<T>() where T : class;
        IQueryable<T> GetQueryable<T>() where T : class;
    }
}