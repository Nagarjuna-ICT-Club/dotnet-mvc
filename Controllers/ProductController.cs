using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using MvcMovie.Models;
using MvcMovie.DAL;

namespace MvcMovie.Controllers;

public class ProductController : Controller
{
    private readonly ProductContext _context;

    public ProductController(ProductContext context)
    {
        _context = context;
    }

    public IActionResult Index()
    {
        var products = _context.Products.ToArray();
        return View(products);
    }

    public IActionResult Detail(int id){
        var product = _context.Products.Find(id)!;
        return View(product);
    }

    public IActionResult NewProduct()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> NewProduct(ProductModel product)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            if (ModelState.IsValid)
            {
                _context.Add(product);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
        }
        catch (Exception)
        {
            System.Console.WriteLine("Exceptional occurred");
        }
        return View();
    }

    public IActionResult UpdateProduct(int id)
    {
        var product = _context.Products.Find(id);
        return View(product);
    }

    [HttpPost]
    public IActionResult UpdateProduct(ProductModel product)
    {
        _context.Products.Update(product);
        _context.SaveChanges();
        return RedirectToAction(nameof(Index));
    }


    public IActionResult Delete(ProductModel product)
    {
        _context.Products.Remove(product);
        _context.SaveChanges();
        return RedirectToAction(nameof(Index));
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
