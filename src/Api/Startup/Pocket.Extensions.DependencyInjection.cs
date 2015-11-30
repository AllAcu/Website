using System;
using Microsoft.Extensions.DependencyInjection;
using Pocket;

namespace AllAcu
{
    public static class Pocket_Extensions_DependencyInjection
    {
        internal static void Populate(this PocketContainer container)
        {
            container.RegisterSingle<IServiceProvider>(c => new PocketServiceProvider(c));
            container.RegisterSingle<IServiceScopeFactory>(c => new ScopeFactory(() => new Scope(c.Resolve<IServiceProvider>())));
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
