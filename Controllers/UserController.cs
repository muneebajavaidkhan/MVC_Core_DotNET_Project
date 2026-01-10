using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SecondProj.Data;
using SecondProj.Models;

namespace SecondProj.Controllers
{
    public class UserController : Controller
    {
        private readonly EcommerceDbContext db;
        public UserController(EcommerceDbContext _context)
        {
            db = _context;
        }
        public IActionResult Index()
        {

            var CategoryProd = new CategoryProductVM()
            {
                Categories = db.Categories.ToList(),
                Products = db.Products.ToList()
            };

            return View(CategoryProd);

        }
        public IActionResult DetailProductPage(int id)
        {
            var product = db.Products.Include(p => p.Category).FirstOrDefault(p => p.Id == id);
            if (product == null) return NotFound();
            ViewBag.RelatedProducts = db.Products
              .Where(p => p.CategoryId == product.CategoryId && p.Id != id)
              .Take(4).ToList();
            return View(product);
        }
    }
}
