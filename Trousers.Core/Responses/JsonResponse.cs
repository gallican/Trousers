namespace Trousers.Core.Responses
{
    public abstract class JsonResponse: Response
    {
        public abstract object Content { get; }
    }
}