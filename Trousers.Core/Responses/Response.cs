namespace Trousers.Core.Responses
{
    public abstract class Response
    {
        public string ResponseType
        {
            get { return GetType().Name; }
        }
    }
}