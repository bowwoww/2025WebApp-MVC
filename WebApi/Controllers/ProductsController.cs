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
        private readonly ProductService _productService;

        public ProductsController(GoodStoreContext2 context,ProductService productService)
        {
            _context = context;
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
            var success = await _productService.PostProducts(products);
            if (!success)
            {
                return BadRequest();
            }


            return CreatedAtAction("GetProducts", new { id = products.ProductID }, products);
        }
        [HttpPost]
        [Route("PostProductsWithDTO")]
        public async Task<ActionResult<ProductPostDTO>> PostProductsWithDTO([FromForm] ProductPostDTO productDto)
        {
            //驗證PK是否重複
            var reslut = await _productService.PostProductWithDTO(productDto);
            if(reslut == 0)
                return BadRequest("Product ID exist.");
            //驗證檔案是否有上傳
            if (reslut == 3)
            {
                return BadRequest("Picture file is invalid, please make sure file type is correct");
            }
            if (reslut == 2)
            {
                return BadRequest("Expection detected");
            }
            return Ok();

        }

        // DELETE: api/Products/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProducts(string id)
        {
            var success = await _productService.DeleteProducts(id);
            if( !success)
            {
                return NotFound();
            }

            return Ok();
        }

        private bool ProductsExists(string id)
        {
            return _context.Product.Any(e => e.ProductID == id);
        }

        

    }
}
