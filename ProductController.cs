using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using SupermarketSystem.Data;

[Authorize]
public class ProductController : Controller
{
    private readonly ApplicationDbContext C_context;

    public ProductController(ApplicationDbContext context)
    {
        C_context = context;
    }

    [AllowAnonymous]
    public IActionResult Index()
    {
        var products = C_context.Products.Where(p => p.IsApproved).ToList();
        return View(products);
    }

    [Authorize(Roles = "Manager, Administrator")]
    public IActionResult Manage()
    {
        var products = C_context.Products.ToList();
        return View(products);
    }

    [Authorize(Roles = "Manager, Administrator")]
    [HttpPost]
    public IActionResult Approve(int id)
    {
        var product = C_context.Products.Find(id);
        if (product != null)
        {
            product.IsApproved = true;
            C_context.SaveChanges();
        }
        return RedirectToAction("Manage");
    }

    [Authorize(Roles = "Administrator")]
    [HttpPost]
    public IActionResult Delete(int id)
    {
        var product = C_context.Products.Find(id);
        if (product != null)
        {
            C_context.Products.Remove(product);
            C_context.SaveChanges();
        }
        return RedirectToAction("Manage");
    }
}
