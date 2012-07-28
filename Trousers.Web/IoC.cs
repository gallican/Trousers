using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Autofac;
using Trousers.Core;
using Trousers.Data.Tfs;
using Trousers.Plugins;
using Module = Autofac.Module;

namespace Trousers.Web
{
    public static class IoC
    {
        public static IContainer LetThereBeIoC()
        {
            var builder = new ContainerBuilder();

            RegisterAllModulesInAppDomain(builder);

            return builder.Build();
        }

        private static void RegisterAllModulesInAppDomain(ContainerBuilder builder)
        {
            var modules = ApplicationAssemblies()
                .SelectMany(a => a.GetExportedTypes())
                .Where(t => t.IsAssignableTo<Module>())
                .Where(t => !t.IsAbstract)
                .Select(t => (Module) Activator.CreateInstance(t))
                .ToArray();

            foreach (var module in modules) builder.RegisterModule(module);
        }

        private static IEnumerable<Assembly> ApplicationAssemblies()
        {
            yield return typeof(CoreModule).Assembly;
            yield return typeof(TfsModule).Assembly;
            yield return typeof(PluginsModule).Assembly;
            yield return typeof(WebModule).Assembly;
        }
    }
}