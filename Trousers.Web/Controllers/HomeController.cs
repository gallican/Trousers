using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Autofac;
using Trousers.Core;
using Trousers.Core.Responses;
using Trousers.Web.Models.Home;

namespace Trousers.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILifetimeScope _scope;
        private readonly IWorkItemProvider _workItemProvider;
        private readonly IWorkItemHistoryProvider _workItemHistoryProvider;

        public HomeController(ILifetimeScope scope, IWorkItemProvider workItemProvider, IWorkItemHistoryProvider workItemHistoryProvider)
        {
            _scope = scope;
            _workItemProvider = workItemProvider;
            _workItemHistoryProvider = workItemHistoryProvider;
        }

        public ActionResult Index()
        {
            return RedirectToAction("Interactive");
        }

        public ActionResult Interactive()
        {
            var model = new InteractiveModel();
            var plugins = _scope.Resolve<IEnumerable<IPlugin>>();
            model.SearchActions = plugins.Select(p => p.GetType().Name).ToArray();
            return View(model);
        }

        [HttpPost]
        public ActionResult Search(string searchAction, string expr)
        {
            //FIXME ugly hack.  remove.
            _workItemProvider.SetQuery(expr);
            _workItemHistoryProvider.SetQuery(expr);

            var pluginInstance = _scope.ResolveNamed<IPlugin>(searchAction);

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