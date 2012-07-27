using Autofac;

namespace Trousers.Web
{
    public static class IoC
    {
        public static IContainer LetThereBeIoC()
        {
            var builder = new ContainerBuilder();
            return builder.Build();
        }
    }
}