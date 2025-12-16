using Microsoft.AspNetCore.Mvc;

namespace SecondProj.Controllers
{
    public class StudentController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult About()
        {
            return View();
        }
        [Route("Student/ContactDetail")]
        public IActionResult Contact()
        {
            return View();
        }
        [Route("Student/Detail/{id}")]
        public IActionResult Detail(int id)
        {
            return Content($"Student detail id = {id}");
        }
        public IActionResult StudentDetail()
        {
            List<string> studName = new List<string> {"Ahmed","Sraha","Fatima","Saad" };
            ViewData["Stud"] = studName;
            return View();

        }


    }
}
