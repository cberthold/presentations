using Autofac;
using EntityFramework.Repository;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaxApp.Common.Queryables
{
    public class RepositoryFactory<TContext> : IRepositoryFactory<TContext>
        where TContext : IDbContext
    {
        private readonly IComponentContext container;
        
        public RepositoryFactory(IComponentContext container)
        {
            this.container = container;
        }

        public IRepository<TContext, T> GetRepository<T>() where T : class
        {
            return container.Resolve<IRepository<TContext, T>>();
        }

        public IDbSet<T> GetSet<T>() where T :class
        {
            return container.Resolve<TContext>().Set<T>();
        }
        
    }
}
