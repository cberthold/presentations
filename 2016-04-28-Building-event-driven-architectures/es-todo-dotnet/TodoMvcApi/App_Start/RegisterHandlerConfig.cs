using Infrastructure.Bus;
using Infrastructure.Config;
using Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Todo.BoundedContext.Handlers;
using Todo.BoundedContext.Projections;

namespace TodoMvcApi
{
    public partial class Startup
    {
        public void ConfigureCommandHandlers(IAppBuilder app)
        {
            var busResolver = new BusResolver(DependencyResolver.Current);
            var registrar = new BusRegistrar(busResolver);

            // setup todo command handlers
            registrar.Register(typeof(TodoCommandHandlers));
            registrar.Register(typeof(TodoListDTOHandlers));
            
        }
    }

    public class BusResolver : IServiceLocator
    {

        private IDependencyResolver _resolver;

        public BusResolver(IDependencyResolver resolver)
        {
            _resolver = resolver;
        }

        public object GetService(Type type)
        {
            var service = _resolver.GetService(type);
            return service;
        }

        public T GetService<T>()
        {
            var service = _resolver.GetService<T>();
            return service;
        }
    }
}