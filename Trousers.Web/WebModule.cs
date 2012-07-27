using Autofac;
using Autofac.Integration.Mvc;

namespace Trousers.Web
{
    public class WebModule: Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            builder.RegisterControllers(typeof(WebModule).Assembly);
        }
    }
}