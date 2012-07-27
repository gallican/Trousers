using System.Web.Mvc;

namespace Trousers.Web.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return RedirectToAction("Interactive");
        }

        public ActionResult Interactive()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Search(string expr)
        {
            if (expr.Contains("json")){
            var result = new
            {
                isJson = true,
                foo = 1,
                bar = "two",
            };

            return Json(result);
            }

            return new ContentResult() { Content = "Hello, world!", ContentType="text/html" };
        }
    }
}