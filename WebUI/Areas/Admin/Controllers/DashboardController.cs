using System.Web.Mvc;

namespace WebUI.Areas.Admin.Controllers
{
    public class DashboardController : Controller
    {
        [Authorize(Roles ="Admin")]
        public ActionResult Index()
        {
            return View();
        }
    }
}