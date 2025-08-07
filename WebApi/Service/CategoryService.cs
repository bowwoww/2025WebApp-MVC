using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApi.DTOs;
using WebApi.Models;

namespace WebApi.Service
{
    public class CategoryService
    {
        private readonly GoodStoreContext _context;
        private readonly FileService _fileService;
        public CategoryService(GoodStoreContext context, FileService fileService)
        {
            _context = context;
            _fileService = fileService;
        }
        public async Task<List<CategoriesDTO>> GetCategory()
        {

            var categories = await _context.Category.Include(c => c.Product) // Eager loading Products
                .AsNoTracking() // Use AsNoTracking for read-only scenarios
                .OrderBy(c => c.CateID) // Order by CateID ascending
                .Select(c => NewCategoryDTO(c)) // Project to DTO
                .ToListAsync();

            return categories;
        }

#nullable enable
        public async Task<CategoriesDTO?> GetCategory(string id)
        {
            var category = await _context.Category.Include(c => c.Product)
                .Where(c => c.CateID == id)
                .Select(c => NewCategoryDTO(c))
                .FirstOrDefaultAsync();
            return category;
        }

        public async Task PutCategory(string id, CategoryPutDTO categoryDTO)
        {

            var category = await _context.Category.FindAsync(id);
            if (category == null)
            {
                throw new KeyNotFoundException($"找不到類別，ID: {id}");
            }

            category.CateName = categoryDTO.CateName;

            //提醒 Entity Framework Core 只會更新被標記為修改的屬性
            _context.Entry(category).Property(c => c.CateName).IsModified = true;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }

        }

        public async Task<Category> PostCategory(CategoryPostDTO category)
        {
            Category cate = new Category
            {
                CateID = category.CateID,
                CateName = category.CateName,
            };
            _context.Category.Add(cate);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                throw;
            }

            return cate;
        }

        public async Task<bool> DeleteCategory(string id)
        {

            var products = await _context.Product.Where(Product => Product.CateID == id).ToListAsync();
            if (products.Count > 0)
            {
                foreach (var p in products)
                {
                    await _fileService.deleteFile(p.ProductID);
                }
                _context.Product.RemoveRange(products);
            }
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                return false;
            }
            Category cate = await _context.Category.FindAsync(id);

            
            _context.Category.Remove(cate);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                return false;
            }

            return true;

        }

        public bool CategoryExists(string id)
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
