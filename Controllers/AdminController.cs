using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.Blazor;
using SecondProj.Data;
using SecondProj.Models;
using System.Collections.Immutable;
using System.Diagnostics;
using System.IO;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using static NuGet.Packaging.PackagingConstants;
using static System.Net.Mime.MediaTypeNames;

namespace SecondProj.Controllers
{
    [Authorize(Policy = "AdminOnly")]
    public class AdminController : Controller
    {
        EcommerceDbContext db = new EcommerceDbContext();
        public IActionResult Index()
        {
            return View();
        }
        private static List<student> students = new List<student>();
        public IActionResult CreateStud()
        {
            return View();
        }
        [HttpPost]
        public IActionResult CreateStud(student stud)
        {
            students.Add(stud);
            TempData["success"] = "Student Record Save Succcessfully";
            return Redirect("ListView");
        }
        public IActionResult ListView()
        {
            return View(students);
        }
        public IActionResult Category()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Category(CategoryViewModel c)
        {
            if (!ModelState.IsValid)
            {
                return View(c);
            }
            //check duplication category
            bool checkdupRec = db.Categories.Any(x => x.CateName.ToLower() == c.CateName.Trim().ToLower());
            if (checkdupRec)
            {
                ModelState.AddModelError(nameof(c.CateName), "Category already exist");
            }
            var dc = new Category();
            dc.CateName = c.CateName;
            db.Categories.Add(dc);
            db.SaveChanges();
            TempData["Success"] = "Category Added Successfully";
            return Redirect("CatList");
        }
        public IActionResult CatList()
        {
            var catList = db.Categories.ToList();
            return View(catList);
        }
        public IActionResult Delete(int id)
        {
            var catDel = db.Categories.FirstOrDefault(x=>x.Id == id);
            if(catDel != null)
            {
                db.Categories.Remove(catDel);
                db.SaveChanges();
            }
            TempData["Deleted"] = "Category Deleted Successfully";
            return RedirectToAction("CatList");
        }
        public IActionResult Edit(int id) {
            var cat = db.Categories.FirstOrDefault(x=>x.Id ==id);
            if(cat == null)
            {
                NotFound();
            }
            return View(cat);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Category c,int id)
        {
            if (!ModelState.IsValid) return View(c);

            var existcat = db.Categories.FirstOrDefault(x=>x.Id == id);
            if(existcat == null)
            {
                return NotFound();
            }

            existcat.CateName =  c.CateName;

            db.Categories.Update(existcat);
            db.SaveChanges();
                return View();
        }
        [HttpGet]
        public IActionResult CreateProd()
        {
            ViewBag.Categorylist = new SelectList(db.Categories, "Id", "CateName");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateProd(Product p,IFormFile ImgFile)
        {
            //step1 ***Get the image name product8.jpg
            var imageName = Path.GetFileName(ImgFile.FileName);
            //step 2 Creates the folder path where the image will be stored e.g wwwroot/Image/
            string imagePath = Path.Combine(HttpContext.Request.PathBase.Value, "wwwroot/Image/");
            //step 3 Combines folder path + image name e.g //wwwroot/Image/Product-8.jpg
            string imagevalue = Path.Combine(imagePath, imageName);
            // step 4 Save the image into the folder
            // Copies the uploaded image into that file
            // Move the image from the form into the folder e.g /Image/product8.jpg
            using (var stream = new FileStream(imagevalue, FileMode.Create))
            {
                ImgFile.CopyTo(stream);
            }

            var dbimage = Path.Combine("/Image/", imageName);
            p.Image = dbimage;

            db.Products.Add(p);
            db.SaveChanges();

            return View();
        }
        public IActionResult ProdList()     
        {
            var datashow = db.Products.Include(x=>x.Category).ToList();
            return View(datashow);
        }
        public IActionResult ProdDelete(int id)
        {
            var ProdDel = db.Products.FirstOrDefault(x => x.Id == id);
            if (ProdDel != null)
            {
                db.Products.Remove(ProdDel);
                db.SaveChanges();
            }
            TempData["Deleted"] = "Product Deleted Successfully";
            return RedirectToAction("ProdList");
        }
        [HttpGet]
        public IActionResult EditProd(int id)
        {
            var prod = db.Products.FirstOrDefault(x => x.Id == id);
            ViewBag.Categorylist = new SelectList(db.Categories, "Id", "CateName");

            if (prod == null)
            {
                return NotFound();
            }
            return View(prod);
        }
        [HttpPost]
        public IActionResult EditProd(Product p, IFormFile ImgFile)
        {
            var existProd = db.Products.Find(p.Id);
            if (existProd == null) {
                NotFound();
            }
            else
            {
                if(ImgFile != null)
                {
                    //step1 ***Get the image name product8.jpg
                    var imageName = Path.GetFileName(ImgFile.FileName);
                    //step 2 Creates the folder path where the image will be stored e.g wwwroot/Image/
                    string imagePath = Path.Combine(HttpContext.Request.PathBase.Value, "wwwroot/Image/");
                    //step 3 Combines folder path + image name e.g //wwwroot/Image/Product-8.jpg
                    string imagevalue = Path.Combine(imagePath, imageName);
                    // step 4 Save the image into the folder
                    // Copies the uploaded image into that file
                    // Move the image from the form into the folder e.g /Image/product8.jpg
                    using (var stream = new FileStream(imagevalue, FileMode.Create))
                    {
                        ImgFile.CopyTo(stream); 
                    }

                    var dbimage = Path.Combine("/Image/", imageName);
                    existProd.Image = dbimage;
                }
                existProd.ProductName = p.ProductName;
                existProd.Description = p.Description;
                existProd.Price = p.Price;
                existProd.Quantity = p.Quantity;
                db.Products.Update(existProd);
                db.SaveChanges();
            }
            TempData["Updated"] = "Product Updated Successfully";
            return RedirectToAction("ProdList");
        }
    }
}
