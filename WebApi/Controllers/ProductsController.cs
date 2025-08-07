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
using WebApi.Service;
using WebApi.QueryParameter;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly GoodStoreContext2 _context;
        private readonly FileService _fileService;
        private readonly ProductService _productService;

        public ProductsController(GoodStoreContext2 context,FileService fileService,ProductService productService)
        {
            _context = context;
            _fileService = fileService;
            _productService = productService;
        }

        [HttpGet("SQL")]
        public async Task<ActionResult<IEnumerable<ProductDTO>>> GetProductFromSQL(string? cateId,string? productName)
        {
            var result = await _productService.GetProductFromSQL(cateId, productName);
            if (result == null || !result.Any())
            {
                return NotFound();
            }
            return result;
        }

        [HttpGet("SQL2")]
        public async Task<ActionResult<IEnumerable<ProductDTO>>> GetProductFromSQL2([FromQuery]string? cateId)
        {
            // Using a stored procedure to fetch products by category ID
            var result = await _productService.GetProductFromSQL(cateId);
            if (result == null || !result.Any())
            {
                return NotFound();
            }
            return result;
        }

        // GET: api/Products
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductDTO>>> GetProducts(ProductParameter productParameter)
        {
            List<ProductDTO> products = await _productService.GetProducts(productParameter);

            if(products == null || products.Count == 0)
            {
                return NotFound();
            }

            return products; 
        }

        // GET: api/Products/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductDTO>> GetProducts(string id)
        {
            var product = await _productService.GetProducts(id);

            if (product == null)
            {
                return NotFound();
            }

            return product;
        }

        // PUT: api/Products/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProducts(string id, ProductPutDTO productDTO)
        {
            if(productDTO == null || String.IsNullOrWhiteSpace(id))
            {
                return BadRequest("Invalid product data.");
            }

            var product = await _productService.GetProducts(id);
            if (product == null) {
                return NotFound();
            }
            
            bool success = await _productService.PutProducts(id, productDTO);
            if (success)
            {
                return Ok(product);
            }

            return BadRequest();
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
                var filename = await _fileService.uploadFile(productDto.Picture, productDto.ProductID);
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
            await _fileService.deleteFile(id); // 刪除相關的圖片檔案
            _context.Product.Remove(products);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ProductsExists(string id)
        {
            return _context.Product.Any(e => e.ProductID == id);
        }

        

    }
}
