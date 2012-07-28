using System.Web;
using Trousers.Core;
using Trousers.Core.Infrastructure;

namespace Trousers.Web.Infrastructure
{
    public class HttpContextFilterExpressionProvider : IFilterExpressionProvider
    {
        public string FilterExpression
        {
            get { return HttpContext.Current.Request.Form["expr"]; }
        }
    }
}