namespace Trousers.Core.Responses
{
    public abstract class Response
    {
        public string ResponseType
        {
            get { return GetType().Name; }
        }

        public long SequenceNumber { get; set; }
    }
}