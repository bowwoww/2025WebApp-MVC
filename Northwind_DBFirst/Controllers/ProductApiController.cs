using Microsoft.AspNetCore.Mvc;
using Northwind_DBFirst.Models;

[ApiController]
[Route("api/[controller]")]
public class ProductsApiController : ControllerBase
{
    private readonly MyDbContext _context;
    public ProductsApiController(MyDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public IActionResult GetProducts()
    {
        var products = _context.Products.ToList();
        return Ok(products);
    }

    [HttpGet("id/{id}/pd")]
    public IActionResult GetProduct(int id)
    {
        var product = _context.Products.Find(id);
        if (product == null)
        {
            return NotFound();
        }
        return Ok(product);
    }
}
