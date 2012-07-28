using Autofac;
using Trousers.Core.DevelopmentStubs;

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

            builder.RegisterGeneric(typeof(MemoryRepository<>))
                .As(typeof(IRepository<>))
                .SingleInstance();
        }
    }
}