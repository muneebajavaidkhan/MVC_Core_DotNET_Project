using Microsoft.AspNetCore.Mvc;

namespace SecondProj.Controllers
{
    public class UserController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
