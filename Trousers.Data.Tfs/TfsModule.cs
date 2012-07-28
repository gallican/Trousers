using Autofac;
using Trousers.Data.Tfs.Infrastructure;

namespace Trousers.Data.Tfs
{
    public class TfsModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            builder.RegisterType<TeamProjectCollectionFactory>();
            builder.RegisterType<WorkItemStoreFactory>();

            builder.Register(c => c.Resolve<TeamProjectCollectionFactory>().Create())
                .AsSelf()
                .SingleInstance();

            builder.Register(c => c.Resolve<WorkItemStoreFactory>().Create())
                .AsSelf()
                .SingleInstance();

            builder.RegisterType<WorkItemFetcher>()
                .AsImplementedInterfaces()
                .SingleInstance();

            builder.RegisterType<WorkItemRepositoryUpdater>()
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();
        }
    }
}