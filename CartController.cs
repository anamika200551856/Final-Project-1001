using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
using SupermarketSystem.Data;

[Authorize(Roles = "Registered user")]
public class CartController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;

    public CartController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    public async Task<IActionResult> Index()
    {
        var user = await _userManager.GetUserAsync(User);
        var cart = _context.Carts.FirstOrDefault(c => c.UserId == user.Id);
        if (cart == null)
        {
            return View(new List<CartItem>());
        }
        var cartItems = _context.CartItems.Where(ci => ci.CartId == cart.Id).ToList();
        return View(cartItems);
    }

    [HttpPost]
    public async Task<IActionResult> AddToCart(int productId)
    {
        var user = await _userManager.GetUserAsync(User);
        var product = _context.Products.Find(productId);
        var cart = _context.Carts.FirstOrDefault(c => c.UserId == user.Id);

        if (cart == null)
        {
            cart = new Cart { UserId = user.Id };
            _context.Carts.Add(cart);
            _context.SaveChanges();
        }

        var cartItem = new CartItem
        {
            ProductId = productId,
            Quantity = 1,
            Price = product.Price,
            CartId = cart.Id
        };

        _context.CartItems.Add(cartItem);
        _context.SaveChanges();

        return RedirectToAction("Index");
    }
}
