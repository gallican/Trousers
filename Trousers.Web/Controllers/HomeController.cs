using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web.Mvc;
using Autofac;
using Trousers.Core.Infrastructure;
using Trousers.Core.Responses;
using Trousers.Web.Models.Home;

namespace Trousers.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILifetimeScope _scope;
        private static long _searchSequenceNumber = 1;

        public HomeController(ILifetimeScope scope)
        {
            _scope = scope;
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
        public ActionResult Search(string searchAction)
        {
            var searchSequenceNumber = Interlocked.Increment(ref _searchSequenceNumber);

            var pluginInstance = _scope.ResolveNamed<IPlugin>(searchAction);

            var response = pluginInstance.Query();
            var jsonResponse = response as JsonResponse;
            if (jsonResponse != null)
            {
                response.SequenceNumber = searchSequenceNumber;
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