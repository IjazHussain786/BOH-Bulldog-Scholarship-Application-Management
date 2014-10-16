using System.Web.Mvc;

namespace BohFoundation.WebApi.Controllers
{
    [AllowAnonymous]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}
