using Autofac;
using Autofac.Core;
using NLog;
using TaxApp.WPF.Common.Command;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BindingFlags = System.Reflection.BindingFlags;
using PropertyInfo = System.Reflection.PropertyInfo;

namespace TaxApp.WPF
{
    public class LoggingModule : Module
    {
        static readonly ConcurrentDictionary<Type, IEnumerable<PropertyInfo>> loggerPropertyMappings
            = new ConcurrentDictionary<Type, IEnumerable<PropertyInfo>>();

        static readonly ConcurrentDictionary<Type, bool> isAsyncMapping
            = new ConcurrentDictionary<Type, bool>();

        static readonly ConcurrentDictionary<Type, Logger> loggerMapping
            = new ConcurrentDictionary<Type, Logger>();

        static readonly Type[] LOGGER_TYPES = new Type[] { typeof(Logger), typeof(ILogger) };

        private static IEnumerable<PropertyInfo> GetLoggerPropertyMappings(Type instanceType)
        {
            return loggerPropertyMappings.GetOrAdd(instanceType, (input) =>
            {
                // Get all the injectable properties to set.
                // If you wanted to ensure the properties were only UNSET properties,
                // here's where you'd do it.
                var properties = input
                  .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                  .Where(p => LOGGER_TYPES.Contains(p.PropertyType) && p.CanWrite && p.GetIndexParameters().Length == 0);

                return properties;
            });
        }

        private static void InjectLoggerProperties(object instance)
        {

            var instanceType = instance.GetType();
            var properties = GetLoggerPropertyMappings(instanceType);

            // Set the properties located.
            foreach (var propToSet in properties)
            {
                propToSet.SetValue(instance, LogManager.GetCurrentClassLogger(instanceType), null);
            }
        }

        private static readonly Type AsyncCommandType = typeof(AsyncCommandFactory<>);

        private static void OnComponentPreparing(object sender, PreparingEventArgs e)
        {
            e.Parameters = e.Parameters.Union(
              new[]
              {
            new ResolvedParameter(
                (p, i) => LOGGER_TYPES.Contains( p.ParameterType),
                (p, i) => {

                    var declaringType = p.Member.DeclaringType;

                    var logger = loggerMapping.GetOrAdd(declaringType, (inputType) =>
                    {
                        var isAsyncMappingCommand = isAsyncMapping.GetOrAdd(inputType, (input) =>
                        {
                            return input.IsGenericType && input.GetGenericTypeDefinition() == AsyncCommandType;
                        });

                        if(isAsyncMappingCommand)
                        {
                            return LogManager.GetCurrentClassLogger(inputType.GetGenericArguments()[0]);
                        }

                        return LogManager.GetCurrentClassLogger(p.Member.DeclaringType);
                    });


                    return logger;

                }),
              });
        }

        protected override void AttachToComponentRegistration(IComponentRegistry componentRegistry, IComponentRegistration registration)
        {
            // Handle constructor parameters.
            registration.Preparing += OnComponentPreparing;

            // Handle properties.
            registration.Activated += (sender, e) => InjectLoggerProperties(e.Instance);
        }
    }
}
