namespace Trousers.Core.Responses
{
    public class ChartResponse : JsonResponse
    {
        private readonly object _content;

        public ChartResponse(object[][] data, object options)
        {
            _content = new
            {
                data,
                options,
            };
        }

        public bool IsChart
        {
            get { return true; }
        }

        public override object Content
        {
            get { return _content; }
        }
    }
}