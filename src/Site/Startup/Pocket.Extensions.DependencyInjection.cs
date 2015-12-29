using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Pocket;

namespace AllAcu
{
    public static class Pocket_Extensions_DependencyInjection
    {
        internal static IServiceProvider Populate(this PocketContainer container, IEnumerable<ServiceDescriptor> services)
        {
            foreach (var service in services)
            {
                container.Register(service);
            }

            container.RegisterSingle<IServiceProvider>(c => new PocketServiceProvider(c));
            container.RegisterSingle<IServiceScopeFactory>(c => new ScopeFactory(() => new Scope(c.Resolve<IServiceProvider>())));

            container.UseAutomaticEnumerableStrategy();
            return container.Resolve<IServiceProvider>();
        }

        internal static void Register(this PocketContainer container, ServiceDescriptor descriptor)
        {
            if (descriptor.ImplementationType != null)
            {
                switch (descriptor.Lifetime)
                {
                    case ServiceLifetime.Singleton:
                        if (descriptor.ServiceType.IsGenericTypeDefinition)
                            container.RegisterGenericSingleton(descriptor.ServiceType, (c, t) => c.Resolve(descriptor.ImplementationType.MakeGenericType(t)));
                        else
                        {
                            if (descriptor.ServiceType != descriptor.ImplementationType)
                                container.RegisterSingle(descriptor.ServiceType, c => c.Resolve(descriptor.ImplementationType));
                            else
                                container.RegisterSingle(c => c.Resolve(descriptor.ImplementationType));
                        }
                        break;
                    case ServiceLifetime.Scoped:
                    case ServiceLifetime.Transient:
                        if (descriptor.ServiceType.IsGenericTypeDefinition)
                            container.RegisterGeneric(descriptor.ServiceType, descriptor.ImplementationType);
                        else
                        {
                            if (descriptor.ServiceType != descriptor.ImplementationType)
                                container.Register(descriptor.ServiceType, c => c.Resolve(descriptor.ImplementationType));
                            else
                            {
                                container.Register(c => c.Resolve(descriptor.ImplementationType));
                            }
                        }
                        break;
                }
            }
            else if (descriptor.ImplementationFactory != null)
            {
                switch (descriptor.Lifetime)
                {
                    case ServiceLifetime.Singleton:
                        if (descriptor.ServiceType.IsGenericTypeDefinition)
                            container.RegisterGenericSingleton(descriptor.ServiceType, (c, t) => descriptor.ImplementationFactory(c.Resolve<IServiceProvider>()));
                        else
                            container.RegisterSingle(descriptor.ServiceType, c => descriptor.ImplementationFactory(c.Resolve<IServiceProvider>()));
                        break;
                    case ServiceLifetime.Scoped:
                    case ServiceLifetime.Transient:
                        container.Register(descriptor.ServiceType, c => descriptor.ImplementationFactory(c.Resolve<IServiceProvider>()));
                        break;
                }
            }
            else
            {
                container.RegisterSingle(descriptor.ServiceType, c => descriptor.ImplementationInstance);
            }
        }

        internal static PocketContainer RegisterGenericSingleton(this PocketContainer container, Type variantsOf, Func<PocketContainer, Type, dynamic> to)
        {
            if (!variantsOf.IsGenericTypeDefinition)
            {
                throw new ArgumentException("Parameter 'variantsOf' is not an open generic type, e.g. typeof(IService<>)");
            }

            var singletons = new ConcurrentDictionary<Type, dynamic>();

            return container.AddStrategy(t =>
            {
                if (t.IsGenericType && t.GetGenericTypeDefinition() == variantsOf)
                {
                    return c => singletons.GetOrAdd(t, to(c, t.GetGenericArguments()[0]));
                }
                return null;
            });
        }

        internal static PocketContainer UseAutomaticEnumerableStrategy(this PocketContainer container)
        {
            return container.AddStrategy(t =>
            {
                if (t.IsGenericType && t.Name == "IEnumerable`1")
                {
                    var type = t.GenericTypeArguments[0];
                    return c =>
                    {
                        var result = Array.CreateInstance(type, 1);
                        result.SetValue(c.Resolve(type), 0);
                        return result;
                    };
                }
                return null;
            });
        }

        internal class ScopeFactory : IServiceScopeFactory
        {
            private readonly Func<IServiceScope> makeScope;

            public ScopeFactory(Func<IServiceScope> makeScope)
            {
                this.makeScope = makeScope;
            }

            public IServiceScope CreateScope()
            {
                return makeScope();
            }
        }

        internal class Scope : IServiceScope
        {
            public Scope(IServiceProvider serviceProvider)
            {
                ServiceProvider = serviceProvider;
            }

            public void Dispose()
            {
            }

            public IServiceProvider ServiceProvider { get; }
        }

        internal class PocketServiceProvider : IServiceProvider
        {
            private readonly PocketContainer container;

            public PocketServiceProvider(PocketContainer container)
            {
                this.container = container;
                container.Register<IServiceProvider>(_ => this);
            }

            public object GetService(Type serviceType)
            {
                return container.Resolve(serviceType);
            }
        }
    }
}
