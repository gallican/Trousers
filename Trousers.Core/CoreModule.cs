using Autofac;
using Trousers.Core.DevelopmentStubs;
using Trousers.Core.Domain.Repositories;
using Trousers.Core.Infrastructure;

namespace Trousers.Core
{
    public class CoreModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            builder.RegisterType<WorkItemHistoryProvider>()
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();

            builder.RegisterType<WorkItemsProvider>()
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();

            builder.RegisterGeneric(typeof(MemoryRepository<>))
                .As(typeof(IRepository<>))
                .SingleInstance();
        }
    }
}