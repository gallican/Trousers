using System.Linq;
using Autofac;
using Trousers.Core;
using Trousers.Core.Infrastructure;

namespace Trousers.Plugins
{
    public class PluginsModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            RegisterPluginTypes(builder);
        }

        private static void RegisterPluginTypes(ContainerBuilder builder)
        {
            var pluginTypes = typeof(PluginsModule).Assembly.GetExportedTypes()
                .Where(t => t.IsAssignableTo<IPlugin>())
                .ToArray();

            foreach (var type in pluginTypes)
            {
                builder.RegisterType(type)
                    .AsImplementedInterfaces()
                    .Named<IPlugin>(type.Name);
            }
        }
    }
}