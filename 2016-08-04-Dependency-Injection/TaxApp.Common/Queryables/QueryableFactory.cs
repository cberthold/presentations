using EntityFramework.Repository;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaxApp.Common.Queryables
{
    public class QueryableFactory<TContext> : IQueryableFactory<TContext>
        where TContext : IDbContext
    {
        readonly IRepositoryFactory<TContext> factory;
        public QueryableFactory(IRepositoryFactory<TContext> factory)
        {
            this.factory = factory;
        }

        public IRepository<TContext, T> GetRepository<T>() where T : class
        {
            return factory.GetRepository<T>();
        }

        public IQueryable<T> GetQueryableNoTracking<T>() where T : class
        {
            return factory.GetSet<T>().AsNoTracking();
        }

        public IQueryable<T> GetQueryable<T>() where T : class
        {
            return factory.GetRepository<T>().AsQueryable();
        }
    }
}
