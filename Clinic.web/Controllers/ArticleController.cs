using Microsoft.AspNetCore.Mvc;

namespace Clinic.web.Controllers
{
    public class ArticleController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
