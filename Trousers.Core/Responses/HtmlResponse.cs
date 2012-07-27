namespace Trousers.Core.Responses
{
    public class HtmlResponse : Response
    {
        private readonly string _html;

        public HtmlResponse(string html)
        {
            _html = html;
        }

        public string Html
        {
            get { return _html; }
        }
    }
}