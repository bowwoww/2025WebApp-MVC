using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using WebApi.DTOs;
using WebApi.Models;
using WebApi.QueryParameter;

namespace WebApi.Service
{
    public class ProductService
    {
        private readonly GoodStoreContext2 _context;
        private readonly FileService _fileService;

        public ProductService(GoodStoreContext2 context, FileService fileService)
        {
            _context = context;
            _fileService = fileService;
        }

        public async Task<List<ProductDTO>> GetProducts(ProductParameter productParameter)
        {
            //分類名稱是其他table ,想回傳時候直接塞入就另外建立一個DTO類別
            var result = _context.Product
                .Include(p => p.Cate) // Eager loading Category
                .AsNoTracking() // Use AsNoTracking for read-only scenarios
                .OrderByDescending(p => p.Price) // Order by Price descending
                .AsQueryable(); // Start with IQueryable to allow further filtering

            if (!string.IsNullOrEmpty(productParameter.cateId))
            {
                result = result.Where(p => p.CateID == productParameter.cateId);
            }
            if (!string.IsNullOrEmpty(productParameter.cateName))
            {
                result = result.Where(p => p.Cate.CateName.Contains(productParameter.cateName));
            }
            if (!string.IsNullOrEmpty(productParameter.productName))
            {
                result = result.Where(p => p.ProductName.Contains(productParameter.productName));
            }
            if (!string.IsNullOrEmpty(productParameter.description))
            {
                result = result.Where(p => p.Description.Contains(productParameter.description));
            }
            if (productParameter.maxPrice.HasValue)
            {
                result = result.Where(p => p.Price <= productParameter.maxPrice && p.Price >= productParameter.minPrice);
            }
            return await result.Select(p => NewProductDTO(p)).ToListAsync();
        }

        public async Task<ProductDTO?> GetProducts(string id)
        {
            var products = await _context.Product
                .Include(p => p.Cate)
                .Where(p => p.ProductID == id)
                .Select(p => NewProductDTO(p))
                .FirstOrDefaultAsync();

            return products;
        }

        public async Task<List<ProductDTO>> GetProductFromSQL(string? cateId)
        {
            // Using a stored procedure to fetch products by category ID
            SqlParameter sqlParameter = new SqlParameter("@cateId", cateId);
            var sql = "exec GetProductsBySQL @cateId";
            
            var result = await _context.ProductDTO.FromSqlRaw<ProductDTO>(sql, sqlParameter)
            .AsNoTracking() // Use AsNoTracking for read-only scenarios
            .ToListAsync();
            return result;
        }

        public async Task<List<ProductDTO>> GetProductFromSQL(string? cateId, string? productName)
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

            return result;
        }

        public async Task<bool> PutProducts(string id, ProductPutDTO productDTO)
        {


            var product = await _context.Product.FindAsync(id);

            // 更新產品資料
            if (productDTO.Price >= 0)
            {
                product.Price = productDTO.Price;
                _context.Entry(product).Property(p => p.Price).IsModified = true;
            }
            if (!String.IsNullOrEmpty(productDTO.Description))
            {
                product.Description = productDTO.Description;
                _context.Entry(product).Property(p => p.Description).IsModified = true;
            }
            if (productDTO.Picture != null && productDTO.Picture.Length != 0)
            {
                var fileName = await _fileService.uploadFile(productDTO.Picture, id);
                product.Picture = fileName;
                _context.Entry(product).Property(p => p.Picture).IsModified = true;
            }
            // _context.Entry(product).State = EntityState.Modified;
            //和 _context.update(product); 效果一樣
            //但如果只更新特定欄位可用_context.Entry(product).Property(p => p.xxx).IsModified = true;
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
    }
}
