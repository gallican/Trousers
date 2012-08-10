namespace Trousers.Core.Responses
{
    public abstract class JsonResponse: Response
    {
        public bool IsJson
        {
            get { return true; }
        }
    }
}