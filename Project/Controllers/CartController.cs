using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Newtonsoft.Json;
using Project.Data;
using Project.Models;

namespace Project.Controllers
{
    public class CartController : Controller
    {
        private readonly ProjectContext _context;
        public CartController(ProjectContext context)
        {
            _context = context;
        }
        private int GetCartItemCount()
        {
            string cart = HttpContext.Session.GetString("Cart");
            List<Items> Cart = (cart != null) ? JsonConvert.DeserializeObject<List<Items>>(cart) : new List<Items>();
            return Cart.Sum(item => item.Quantity);
        }

        public IActionResult Cart()
        {
            string cart = base.HttpContext.Session.GetString("Cart");
            List<Items> Cart = (cart != null) ? JsonConvert.DeserializeObject<List<Items>>(cart) : new List<Items>();
            ViewBag.CartItemCount = GetCartItemCount();
            return View(Cart);
        }

        [HttpPost]
        public IActionResult Cart(int PId)
        {
            string cart = base.HttpContext.Session.GetString("Cart");
            List<Items> Cart = (cart != null) ? JsonConvert.DeserializeObject<List<Items>>(cart) : new List<Items>();
            bool found = false;
            foreach (Items cartItem in Cart)
            {
                if (cartItem.Product.PId == PId)
                {
                    cartItem.Quantity++;
                    found = true;
                    break;
                }
            }
            if (!found)
            {
                Items item = new Items();
                item.Product = _context.Product.Find(PId);
                item.Quantity = 1;
                Cart.Add(item);
            }
            base.HttpContext.Session.SetString("Cart", JsonConvert.SerializeObject(Cart));
            ViewBag.CartItemCount = GetCartItemCount();
            return RedirectToAction("Cart");
        }

        public IActionResult RemoveFromCart(int PId)
        {
            string cart = base.HttpContext.Session.GetString("Cart");
            if (cart != null)
            {
                List<Items> Cart = JsonConvert.DeserializeObject<List<Items>>(cart);
                Items itemToRemove = Cart.FirstOrDefault((Items item) => item.Product.PId == PId);
                if (itemToRemove != null)
                {
                    Cart.Remove(itemToRemove);
                    base.HttpContext.Session.SetString("Cart", JsonConvert.SerializeObject(Cart));
                }
            }
            ViewBag.CartItemCount = GetCartItemCount();
            return RedirectToAction("Cart");
        }

        [HttpPost]
        public IActionResult UpdateQuantity(int PId, int Quantity)
        {
            string cart = base.HttpContext.Session.GetString("Cart");
            if (cart != null)
            {
                List<Items> Cart = JsonConvert.DeserializeObject<List<Items>>(cart);
                Items itemToUpdate = Cart.FirstOrDefault((Items item) => item.Product.PId == PId);
                if (itemToUpdate != null)
                {
                    itemToUpdate.Quantity = Quantity;
                    base.HttpContext.Session.SetString("Cart", JsonConvert.SerializeObject(Cart));
                }
            }
            return RedirectToAction("Cart");
        }

        public IActionResult Checkout()
        {
            if (!User.Identity.IsAuthenticated)
            {
                // Redirect to login page if user is not authenticated
                return RedirectToAction("Loginn", "Account", new { returnUrl = "/Cart/Checkout" });
            }

            var cart = HttpContext.Session.GetString("Cart");

            if (cart != null)
            {
                // Retrieve cart data from session
                List<Items> cartItems = JsonConvert.DeserializeObject<List<Items>>(cart);

                

                // Pass the address model and cart items to the view
                ViewBag.CartItems = cartItems;
                return View(cartItems);
            }
            // Handle case where cart is empty
            return View();
        }
    }
}