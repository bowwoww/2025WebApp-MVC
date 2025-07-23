using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApi.Models;
using WebApi.DTOs;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly GoodStoreContext _context;

        public CategoriesController(GoodStoreContext context)
        {
            _context = context;
        }

        // GET: api/Categories
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CategoriesDTO>>> GetCategory()
        {

            var categories = await _context.Category.Include(c => c.Product) // Eager loading Products
                .AsNoTracking() // Use AsNoTracking for read-only scenarios
                .OrderBy(c => c.CateID) // Order by CateID ascending
                .Select(c => NewCategoryDTO(c)) // Project to DTO
                .ToListAsync();

            return categories;
        }

        // GET: api/Categories/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CategoriesDTO>> GetCategory(string id)
        {
            var category = await _context.Category.Include(c => c.Product).Where(c => c.CateID == id).Select(c => NewCategoryDTO(c)).FirstOrDefaultAsync();

            if (category == null)
            {
                return NotFound();
            }

            return category;
        }

        // PUT: api/Categories/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCategory(string id, Category category)
        {
            if (id != category.CateID)
            {
                return BadRequest();
            }

            _context.Entry(category).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CategoryExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Categories
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Category>> PostCategory([FromForm]Category category)
        {
            _context.Category.Add(category);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (CategoryExists(category.CateID))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetCategory", new { id = category.CateID }, category);
        }

        // DELETE: api/Categories/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(string id)
        {
            var category = await _context.Category.FindAsync(id);
            if (category == null)
            {
                return NotFound();
            }

            _context.Category.Remove(category);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CategoryExists(string id)
        {
            return _context.Category.Any(e => e.CateID == id);
        }

        private static CategoriesDTO NewCategoryDTO(Category c)
        {
            return new CategoriesDTO
            {
                CateID = c.CateID,
                CateName = c.CateName,
                Product = c.Product.Select(p => new Product
                {
                    ProductID = p.ProductID,
                    ProductName = p.ProductName,
                    Price = p.Price,
                    Description = p.Description,
                    Picture = p.Picture,
                    CateID = p.CateID
                }).ToList()
            };
        }
    }
}
