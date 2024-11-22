using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Project.Data;
using Project.Models;
using System.Diagnostics;

namespace Project.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ProjectContext _context;

        public HomeController(ILogger<HomeController> logger, ProjectContext context)
        {
            _logger = logger;
            _context = context;
        }
        public IActionResult Index()
        {
            var fetch = _context.Product.Take(6).ToList();
            string cart = HttpContext.Session.GetString("Cart");
            List<Items> cartItems = (cart != null) ? JsonConvert.DeserializeObject<List<Items>>(cart) : new List<Items>();
            int cartItemCount = cartItems.Sum(item => item.Quantity);
            ViewBag.CartItemCount = cartItemCount;
            return View(fetch);
        }
      
        public IActionResult Tshirt()
        {
            string cart = HttpContext.Session.GetString("Cart");
            List<Items> cartItems = (cart != null) ? JsonConvert.DeserializeObject<List<Items>>(cart) : new List<Items>();
            int cartItemCount = cartItems.Sum(item => item.Quantity);
            ViewBag.CartItemCount = cartItemCount;
            var tshirts = _context.Product.Where(p => p.CatgId == 1).ToList();
            return View(tshirts);
        }
        public IActionResult Caps()
        {
            string cart = HttpContext.Session.GetString("Cart");
            List<Items> cartItems = (cart != null) ? JsonConvert.DeserializeObject<List<Items>>(cart) : new List<Items>();
            int cartItemCount = cartItems.Sum(item => item.Quantity);
            ViewBag.CartItemCount = cartItemCount;
            var caps = _context.Product.Where(p => p.CatgId == 2).ToList();
            return View(caps);
        }
        public IActionResult Mugs()
        {
            string cart = HttpContext.Session.GetString("Cart");
            List<Items> cartItems = (cart != null) ? JsonConvert.DeserializeObject<List<Items>>(cart) : new List<Items>();
            int cartItemCount = cartItems.Sum(item => item.Quantity);
            ViewBag.CartItemCount = cartItemCount;
            var mugs = _context.Product.Where(p => p.CatgId == 3).ToList();
            return View(mugs);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
