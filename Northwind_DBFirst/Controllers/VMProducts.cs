using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Northwind_DBFirst.Models;

public class ProductCategoryController : Controller
{
    private readonly MyDbContext _context;
    public ProductCategoryController(MyDbContext context)
    {
        _context = context;
    }

    public IActionResult Index()
    {
        var vm = new VMProducts
        {
            Categories = _context.Categories.ToList(),
            selectedCategory = 1, // 預設選擇第一個類別
            Products = _context.Products
                .Include(p => p.Category)
                .Where(p => p.CategoryId == 1)
                .ToList()
        };


        return View(vm);
    }

    public IActionResult ProductTable(int? categoryId)
    {
        var prod = _context.Products.Where(p => p.CategoryId == categoryId)
            .Include(p => p.Category)
            .ToList();
        return PartialView("ProductTable", prod);
    }

    public IActionResult ProductApi(int? categoryId)
    {

        //不要傳入picture
        var products = _context.Products
            .Where(p => p.CategoryId == categoryId)
            .Include(p => p.Category)
            .Select(p => new
            {
                p.ProductId,
                p.ProductName,
                p.UnitPrice,
                p.UnitsInStock,
                CategoryName = p.Category.CategoryName,
                p.Discontinued
            })
            .ToList();
        return Json(products);
    }

    public IActionResult CategoryApi()
    {
        var categories = _context.Categories
            .Select(c => new
            {
                c.CategoryId,
                c.CategoryName
            })
            .ToList();
        return Json(categories);
    }
}
