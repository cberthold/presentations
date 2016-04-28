using Autofac;
using Autofac.Core;
using FluentValidation;
using Infrastructure.Bus;
using Infrastructure.Command;
using Infrastructure.Domain;
using Infrastructure.Events;
using Infrastructure.EventStore;
using Infrastructure.Repository;
using Infrastructure.Validation;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using Todo.BoundedContext.Commands;
using Todo.BoundedContext.Handlers;
using Todo.BoundedContext.Projections;
using Todo.BoundedContext.ReadModel;

namespace TodoMvcApi.Modules
{
    public class TodoMvcModule : Module
    {
        private IEnumerable<System.Reflection.Assembly> GetAvailableAssemblies()
        {
            yield return typeof(AddTodoItem).Assembly;
            yield return typeof(Infrastructure.Exceptions.ValidationException).Assembly;
        }

        protected override void Load(ContainerBuilder builder)
        {
            var assembliesToCheck =
                GetAvailableAssemblies()
                .ToArray();

            builder.RegisterType<InProcessBus>()
                .AsSelf()
                .As<ICommandSender>()
                .As<IEventPublisher>()
                .As<IHandlerRegistrar>()
                .SingleInstance();
            // in memory event store
            //builder.RegisterType<InMemoryEventStore>()
            //    .As<IEventStore>()
            //    .SingleInstance();

            // raven db event store
            //builder.RegisterType<RavenDbEventStore>()
            //    .As<IEventStore>()
            //    .InstancePerRequest();

            // sql event store
            builder.Register((ctx) =>
            {
                var connectionString =
                    ConfigurationManager
                        .ConnectionStrings["DefaultConnection"]
                        .ConnectionString;

                return new SqlConnection(connectionString);
            })
            .Named<SqlConnection>("EventStore")
            .InstancePerDependency()
            .AsSelf();

            builder.RegisterType<SqlEventStore>()
                .WithProperty(new ResolvedParameter(
                    (pi, ctx) => { return pi.ParameterType == typeof(SqlConnection); },
                    (pi, ctx) => { return ctx.ResolveNamed<SqlConnection>("EventStore"); }
                    ))
                .As<IEventStore>()
                .InstancePerRequest();

            builder.RegisterType<Session>()
                .As<ISession>()
                .InstancePerRequest();

            builder.Register((ctx) =>
            {
                var eventStore = ctx.Resolve<IEventStore>();
                var publisher = ctx.Resolve<IEventPublisher>();

                //return new CacheRepository(new Repository(eventStore, publisher), eventStore);
                return new Repository(eventStore, publisher);
            })
            .As<IRepository>()
            .InstancePerRequest();

            builder.RegisterAssemblyTypes(assembliesToCheck)
               .AsClosedTypesOf(typeof(IValidator<>))
               .InstancePerRequest();

            builder.RegisterType<ValidationFactory>()
                .AsImplementedInterfaces()
                .InstancePerRequest();

            builder.RegisterType<CommandDispatcher>()
                .AsSelf()
                .InstancePerRequest();

            builder.RegisterType<TodoCommandHandlers>()
                .AsImplementedInterfaces()
                .AsSelf()
                .InstancePerRequest();

            builder.RegisterType<TodoListDTOHandlers>()
                .AsImplementedInterfaces()
                .AsSelf()
                .InstancePerRequest();

            builder.RegisterType<RebuildReadModelCommandHandlers>()
                .AsImplementedInterfaces()
                .AsSelf()
                .InstancePerRequest();

            builder.RegisterGeneric(typeof(MongoReadRepository<>))
                .As(typeof(IReadRepository<>));
            builder.RegisterGeneric(typeof(RavenDbReadRepository<>))
                .As(typeof(IReadRepository<>))
                .InstancePerRequest();

            builder.RegisterType<TodoListReadModelFacade>()
                .AsImplementedInterfaces()
                .InstancePerRequest();

            base.Load(builder);
        }
    }
}