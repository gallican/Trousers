using Trousers.Core.Responses;

namespace Trousers.Core.Infrastructure
{
    public interface IPlugin
    {
        Response Query();
    }
}