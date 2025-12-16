using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using SecondProj.Models;

namespace SecondProj.Controllers
{
    public class HomeController : Controller
    {
        public class employee {
            public int EmployeeID { get; set; }
            public string EmployeeName { get; set; }
            public int Age { get; set; }
        }
        IList<employee> employeeList;
        public HomeController()
        {
            employeeList = new List<employee>() { 
                new employee(){ EmployeeID = 1,EmployeeName = "Steve",Age = 21},
                new employee(){EmployeeID = 2,EmployeeName= "John Doe",Age = 25}

            };

        }
        public IActionResult EmployeeData()
        {
            ViewBag.TotalEmployee = employeeList.Count();
            return View();
        }
        public IActionResult Index()
            
        {
            ViewData["Name"] = "Sana";
            return View();
        }

        public IActionResult Save()
        {
            TempData["Success"] = "Student Data save successfully";
            return Redirect("Result");
        }

        public IActionResult Result()
        {
            return View();
        }


        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
