using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EShop.Areas.Admin.Controllers
{
    [Route("admin.html", Name = "AdminIndex")]
    [Area("Admin")]
    [Authorize()]
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}