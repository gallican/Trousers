using System.Web.Mvc;
using Autofac;
using Trousers.Core;
using Trousers.Core.Responses;

namespace Trousers.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILifetimeScope _scope;
        private readonly IWorkItemHistoryProvider _workItemHistoryProvider;

        public HomeController(ILifetimeScope scope, IWorkItemHistoryProvider workItemHistoryProvider)
        {
            _scope = scope;
            _workItemHistoryProvider = workItemHistoryProvider;
        }

        public ActionResult Index()
        {
            return RedirectToAction("Interactive");
        }

        public ActionResult Interactive()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Search(string plugin, string expr)
        {
            _workItemHistoryProvider.SetQuery(expr);
            var pluginInstance = _scope.ResolveNamed<IPlugin>(plugin);

            var response = pluginInstance.Query();
            var jsonResponse = response as JsonResponse;
            if (jsonResponse != null)
            {
                return Json(jsonResponse);
            }

            var htmlResponse = (HtmlResponse) response;
            return new ContentResult {ContentType = "text/html", Content = htmlResponse.Html};
        }

        public ActionResult Test()
        {
            return View();
        }
    }
}