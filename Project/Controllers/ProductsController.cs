using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Project.Data;
using Project.Models;
using System.Linq;
namespace Project.Controllers
{
    public class ProductsController : Controller
    {
        private readonly ProjectContext _context;

        public ProductsController(ProjectContext context)
        {
            _context = context;
        }

        // GET: Products
        public async Task<IActionResult> Index()
        {
            var projectContext = _context.Product.Include(p => p.Category);
            return View(await projectContext.ToListAsync());
        }

        [HttpGet]
        public IActionResult Search(string searchString)
        {
            var fetch = _context.Product.ToList();
            if (!String.IsNullOrEmpty(searchString))
            {
                // Split the search string into individual words
                string[] searchWords = searchString.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                // Retrieve products whose names contain any of the search words as substrings
                fetch = fetch.Where(product => searchWords.Any(word =>
                    product.PName.Contains(word, StringComparison.OrdinalIgnoreCase))).ToList();
            }

            if (fetch.Count == 0)
            {
                // If no products are found, return a view with a message indicating no products were found
                return View("NoProductsFound");
            }

            return View(fetch);
        }






        // GET: Products/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var product = await _context.Product
                .Include(p => p.Category)
                .FirstOrDefaultAsync(m => m.PId == id);
            if (product == null)
            {
                return NotFound();
            }

            var randomProducts = await _context.Product
            .OrderBy(p => Guid.NewGuid()) // Shuffle the products
            .Take(4) // Take 4 random products
            .ToListAsync();

            ViewBag.RandomProducts = randomProducts;
            string cart = HttpContext.Session.GetString("Cart");
            List<Items> cartItems = (cart != null) ? JsonConvert.DeserializeObject<List<Items>>(cart) : new List<Items>();
            int cartItemCount = cartItems.Sum(item => item.Quantity);
            ViewBag.CartItemCount = cartItemCount;

            return View(product);
        }

        // GET: Products/Create
        public IActionResult Create()
        {
            ViewBag.getCatgId = _context.PCategory.ToList();
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PId,PName,Price,CatgId,img_name")] Product product)
        {
            if (ModelState.IsValid)
            {
                _context.Add(product);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CatgId"] = new SelectList(_context.PCategory, "CatgId", "CatgId", product.CatgId);
            return View(product);
        }

        // GET: Products/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Product.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            ViewData["CatgId"] = new SelectList(_context.PCategory, "CatgId", "CatgId", product.CatgId);
            return View(product);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("PId,PName,Price,CatgId,img_name")] Product product)
        {
            if (id != product.PId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(product);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.PId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["CatgId"] = new SelectList(_context.PCategory, "CatgId", "CatgId", product.CatgId);
            return View(product);
        }

        // GET: Products/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Product
                .Include(p => p.Category)
                .FirstOrDefaultAsync(m => m.PId == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = await _context.Product.FindAsync(id);
            if (product != null)
            {
                _context.Product.Remove(product);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductExists(int id)
        {
            return _context.Product.Any(e => e.PId == id);
        }

        public IActionResult PCategory()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> PCategory(PCategory model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }


            // Create a new user object
            var newU = new PCategory
            {
                CatgName = model.CatgName
            };

            // Add the new user to the DbContext and save changes
            _context.PCategory.Add(newU);
            await _context.SaveChangesAsync();

            // Redirect the user to a confirmation page or dashboard
            return RedirectToAction("Index", "Home");
        }
    }
}
