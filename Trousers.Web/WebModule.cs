using Autofac;
using Autofac.Integration.Mvc;
using Trousers.Web.Infrastructure;
using Trousers.Web.Infrastructure.Settings;

namespace Trousers.Web
{
    public class WebModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            builder.RegisterControllers(typeof(WebModule).Assembly);

            builder.RegisterType<WebConfigSettings>()
                .AsImplementedInterfaces()
                .SingleInstance();

            builder.RegisterType<AutofacEventBroker>()
                .AsImplementedInterfaces()
                .SingleInstance();

            builder.RegisterType<HttpContextFilterExpressionProvider>()
                .AsImplementedInterfaces()
                .SingleInstance();
        }
    }
}