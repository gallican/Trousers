using System.Collections.Generic;
using System.Linq;
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