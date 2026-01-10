using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SecondProj.Data;
using SecondProj.Models;

namespace SecondProj.Controllers
{
    public class CartController : Controller
    {
        private readonly EcommerceDbContext db;

        public CartController(EcommerceDbContext context)
        {
            db = context;
        }

        // ===============================
        // ADD TO CART (FIXED)
        // ===============================
        public IActionResult AddToCart(int productId, int qty = 1)
        {
            int userId = int.Parse(User.FindFirst("UserId")?.Value ?? "0");
            if (userId == 0)
                return RedirectToAction("Login", "Account");

            var product = db.Products.FirstOrDefault(p => p.Id == productId);
            if (product == null)
                return NotFound();

            // 🔴 CHECK IF PRODUCT ALREADY IN CART
            var cartItem = db.Carts
                .FirstOrDefault(c => c.UserId == userId && c.ProductId == productId);

            if (cartItem != null)
            {
                // ✅ PRODUCT EXISTS → INCREASE QUANTITY
                cartItem.Quantity += qty;
                cartItem.Tprice = cartItem.Quantity * product.Price;

                db.Carts.Update(cartItem);
            }
            else
            {
                // ✅ PRODUCT NOT IN CART → ADD NEW
                Cart cart = new Cart
                {
                    ProductId = productId,
                    UserId = userId,
                    Quantity = qty,
                    Tprice = product.Price * qty
                };

                db.Carts.Add(cart);
            }

            db.SaveChanges();
            return RedirectToAction("CartList");
        }

        // ===============================
        // CART LIST
        // ===============================
        public IActionResult CartList()
        {
            int userId = int.Parse(User.FindFirst("UserId")?.Value ?? "0");
            if (userId == 0)
                return RedirectToAction("Login", "Account");

            var items = db.Carts
                .Include(c => c.Product)
                .Where(c => c.UserId == userId)
                .ToList();

            return View(items);
        }

        // ===============================
        // REMOVE ITEM
        // ===============================
        public IActionResult Remove(int id)
        {
            var item = db.Carts.Find(id);
            if (item != null)
            {
                db.Carts.Remove(item);
                db.SaveChanges();
            }

            return RedirectToAction("CartList");
        }
        // INCREASE QUANTITY //Decrease Quantity in cart list


        [HttpPost]
        public IActionResult UpdateCart([FromBody] CardUpdateVM cart)
        {
            // Load cart with product included
            var carts = db.Carts
                          .Include(c => c.Product)
                          .FirstOrDefault(c => c.Id == cart.CartId);

            if (carts != null && carts.Product != null)
            {
                // Update quantity
                carts.Quantity = cart.Qty;

                // Update total price
                carts.Tprice = carts.Quantity * carts.Product.Price;

                db.SaveChanges();
            }

            return Ok();
        }
        //CheckOut Page get method 
        public IActionResult Checkout()
        {
            // 1️⃣ User ID nikalna (jo logged-in hai)
            int userId = int.Parse(User.FindFirst("UserId")?.Value ?? "0");

            // 2️⃣ Agar user login nahi hai to login page par bhej do
            if (userId == 0)
                return RedirectToAction("Login", "Account");

            // 3️⃣ Cart items fetch karo jo current user ke liye hain
            var cartItems = db.Carts
                              .Include(c => c.Product) // Taake product ka name aur price access ho
                              .Where(c => c.UserId == userId)
                              .ToList();

            // 4️⃣ Checkout view me cart items bhejo
            return View(cartItems);
        }

        [HttpPost]
        public IActionResult PlaceOrder(string shippingAddress, string paymentMethod)
        {
            int userId = int.Parse(User.FindFirst("UserId")?.Value ?? "0");
            if (userId == 0) return RedirectToAction("Login", "Account");

            var cartItems = db.Carts.Where(c => c.UserId == userId).ToList();
            if (!cartItems.Any()) return RedirectToAction("CartList");

            foreach (var item in cartItems)
            {
                Order order = new Order
                {
                    UserId = userId,
                    ProductId = item.ProductId,
                    ShippingAddress = shippingAddress,
                    PaymentMethod = paymentMethod,
                    Tprice = item.Tprice
                };
                db.Orders.Add(order);
            }

            db.SaveChanges();

            // Clear cart
            db.Carts.RemoveRange(cartItems);
            db.SaveChanges();

            return RedirectToAction("OrderSuccess");
        }
        public IActionResult OrderSuccess()
        {
            return View();
        }


    }
}
