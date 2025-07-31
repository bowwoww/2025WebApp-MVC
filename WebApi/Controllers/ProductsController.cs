using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApi.Models;
using WebApi.DTOs;
using Microsoft.Data.SqlClient;
using Microsoft.IdentityModel.Tokens;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly GoodStoreContext2 _context;

        public ProductsController(GoodStoreContext2 context)
        {
            _context = context;
        }

        [HttpGet("SQL")]
        public async Task<ActionResult<IEnumerable<ProductDTO>>> GetProductFromSQL(string? cateId,string? productName)
        {
            var sql = @"SELECT p.ProductID, p.ProductName, p.Price, p.Description, p.Picture, 
                            c.CateID, c.CateName
                     FROM Product p
                     JOIN Category c ON p.CateID = c.CateID 
                     Where 1 = 1 ";

            List<SqlParameter> parameters = new List<SqlParameter>();

            if (!string.IsNullOrEmpty(cateId))
            {
                sql += " and c.CateID = @cateId";
                parameters.Add(new SqlParameter("@cateId", cateId));
            }

            if (!string.IsNullOrEmpty(productName))
            {
                sql += " and p.ProductName like @productName";
                parameters.Add(new SqlParameter("@productName", $"%{productName}%"));
            }

            var result = await _context.ProductDTO.FromSqlRaw<ProductDTO>(sql, parameters.ToArray())
            .AsNoTracking() // Use AsNoTracking for read-only scenarios
            .ToListAsync();
            if (_context.Product == null)
            {
                return NotFound();
            }
            return result;
        }

        [HttpGet("SQL2")]
        public async Task<ActionResult<IEnumerable<ProductDTO>>> GetProductFromSQL2([FromQuery]string? cateId)
        {
            // Using a stored procedure to fetch products by category ID
            List<SqlParameter> parameter = new List<SqlParameter>();
            var sql = "exec GetProductsBySQL @cateId";
            parameter.Add(new SqlParameter("@cateId",cateId));
            var result = await _context.ProductDTO.FromSqlRaw<ProductDTO>(sql,parameter)
            .AsNoTracking() // Use AsNoTracking for read-only scenarios
            .ToListAsync();
            if (_context.Product == null)
            {
                return NotFound();
            }
            return result;
        }

        // GET: api/Products
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductDTO>>> GetProducts(string? cateId,string? cateName,string? productName,string? description,decimal? maxPrice, decimal? minPrice=0)
        {
            //分類名稱是其他table ,想回傳時候直接塞入就另外建立一個DTO類別
            var result = _context.Product
                .Include(p => p.Cate) // Eager loading Category
                .AsNoTracking() // Use AsNoTracking for read-only scenarios
                .OrderByDescending(p => p.Price) // Order by Price descending
                .AsQueryable(); // Start with IQueryable to allow further filtering

            if (!string.IsNullOrEmpty(cateId))
            {
                result = result.Where(p => p.CateID == cateId);
            }
            if(!string.IsNullOrEmpty(cateName))
            {
                result = result.Where(p => p.Cate.CateName.Contains(cateName));
            }
            if(!string.IsNullOrEmpty(productName))
            {
                result = result.Where(p => p.ProductName.Contains(productName));
            }
            if(!string.IsNullOrEmpty(description))
            {
                result = result.Where(p => p.Description.Contains(description));
            }
            if (maxPrice.HasValue)
            {
                result = result.Where(p => p.Price <= maxPrice && p.Price >= minPrice);
            }
            return await result.Select(p => NewProductDTO(p)).ToListAsync();
        }

        // GET: api/Products/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductDTO>> GetProducts(string id)
        {
            var products = await _context.Product
                .Include(p => p.Cate)
                .Where(p => p.ProductID == id)
                .Select(p => NewProductDTO(p))
                .FirstOrDefaultAsync();

            if (products == null)
            {
                return NotFound();
            }

            return products;
        }

        // PUT: api/Products/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProducts(string id, Product products)
        {
            if (id != products.ProductID)
            {
                return BadRequest();
            }

            _context.Entry(products).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductsExists(id))
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

        // POST: api/Products
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Product>> PostProducts([FromForm] Product products)
        {
            _context.Product.Add(products);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (ProductsExists(products.ProductID))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetProducts", new { id = products.ProductID }, products);
        }
        [HttpPost]
        [Route("PostProductsWithDTO")]
        public async Task<ActionResult<ProductPostDTO>> PostProductsWithDTO([FromForm] ProductPostDTO productDto)
        {
            //驗證PK是否重複
            if(_context.Product.AsNoTracking().Any(p => p.ProductID == productDto.ProductID))
                return BadRequest("Product ID exist.");
            //驗證檔案是否有上傳
            if (productDto.Picture == null || productDto.Picture.Length == 0)
            {
                return BadRequest("Picture is required.");
            }
            else
            {
                var filename = await uploadFile(productDto.Picture, productDto.ProductID);
                // 確保上傳檔案為圖片格式 , 否則回傳BadRequest
                if (string.IsNullOrEmpty(filename))
                { 
                    return BadRequest("Invalid file format. Only images are allowed.");
                }

                // 建立新的 Product 實體並填充資料
                var product = new Product
                {
                    ProductID = productDto.ProductID,
                    ProductName = productDto.ProductName,
                    Price = productDto.Price,
                    Description = productDto.Description,
                    Picture = filename,
                    CateID = productDto.CateID
                };
                _context.Product.Add(product);
                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateException)
                {
                    throw;
                }
                return CreatedAtAction("GetProducts", new { id = product.ProductID }, product);
            }
        }

        // DELETE: api/Products/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProducts(string id)
        {
            var products = await _context.Product.FindAsync(id);
            if (products == null)
            {
                return NotFound();
            }

            _context.Product.Remove(products);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ProductsExists(string id)
        {
            return _context.Product.Any(e => e.ProductID == id);
        }

        private static ProductDTO NewProductDTO(Product product)
        {
            return new ProductDTO
            {
                ProductID = product.ProductID,
                ProductName = product.ProductName,
                Price = product.Price,
                Description = product.Description,
                Picture = product.Picture,
                CateID = product.CateID,
                CateName = product.Cate.CateName, // Assuming CateName is a property in Category
            };
        }

        private async Task<string> uploadFile(IFormFile file,string productId)
        {                 
            // 確保上傳檔案為圖片格式 , 否則回傳BadRequest
            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
            // 檢查檔案副檔名是否在允許的範圍內(轉換為小寫以避免大小寫問題)
            var fileExtension = Path.GetExtension(file.FileName).ToLowerInvariant();
            if (!allowedExtensions.Contains(fileExtension))
            {
                return "";
            }
            // 如要避免因路徑不存在而導致錯誤，可以先檢查並創建目錄
            var directoryPath = Path.Combine("wwwroot", "ProductPhotos");
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }

            // 使用 IFormFile 的 FileName 屬性來獲取檔案的副檔名
            var filename = productId + Path.GetExtension(file.FileName);
            var filePath = Path.Combine(directoryPath, filename);
            // using 確保在使用完檔案流後釋放資源,使用 FileStream 來寫入檔案
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }
            return filename;
            // 返回檔案名稱以便存儲到資料庫
        }
    }
}
