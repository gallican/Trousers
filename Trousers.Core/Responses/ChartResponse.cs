namespace Trousers.Core.Responses
{
    public class ChartResponse : JsonResponse
    {
        private readonly object[][] _data;
        private readonly object _options;

        public ChartResponse(object[][] data, object options)
        {
            _data = data;
            _options = options;
        }

        public bool IsChart
        {
            get { return true; }
        }

        public object[][] Data
        {
            get { return _data; }
        }

        public object Options
        {
            get { return _options; }
        }
    }
}