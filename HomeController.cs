using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore; 
using SupermarketSystem.Data;
using SupermarketSystem.Models;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;

namespace SupermarketSystem.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        // Action for displaying the home page
        public IActionResult Index()
        {
            // Fetch products to display on the homepage
            var products = _context.Products.ToList();
            return View(products);
        }

        // Action for displaying user profile or account details
        [Authorize(Roles = "RegisteredUser")]
        public IActionResult Profile()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                return Unauthorized();
            }

            var user = _context.Users.FirstOrDefault(u => u.Id == userId);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // Action for displaying a list of products (for example, for browsing)
        public IActionResult Products()
        {
            var products = _context.Products.ToList();
            return View(products);
        }

        // Action for displaying product details
        public IActionResult ProductDetails(int id)
        {
            var product = _context.Products.FirstOrDefault(p => p.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // Action for managing the shopping cart
        [Authorize(Roles = "RegisteredUser")]
        public IActionResult Cart()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                return Unauthorized();
            }

            var cart = _context.Cart
                .Include(c => c.CartItems)
                .ThenInclude(ci => ci.Product)
                .FirstOrDefault(c => c.UserId == userId);

            if (cart == null)
            {
                return NotFound();
            }

            return View(cart);
        }

        // Action for handling orders
        [Authorize(Roles = "RegisteredUser")]
        public IActionResult Orders()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                return Unauthorized();
            }

            var orders = _context.Orders
                .Where(o => o.UserId == userId)
                .ToList();

            return View(orders);
        }

        // Action for displaying privacy information
        public IActionResult Privacy()
        {
            return View();
        }

        // Action for displaying error information
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
