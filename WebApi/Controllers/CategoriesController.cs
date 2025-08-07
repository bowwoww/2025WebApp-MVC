using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApi.Models;
using WebApi.DTOs;
using WebApi.Service;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly GoodStoreContext _context;
        private readonly FileService _fileService;
        private readonly CategoryService _categoryService;

        public CategoriesController(GoodStoreContext context,FileService fileService,CategoryService categoryService)
        {
            _context = context;
            _fileService = fileService;
            _categoryService = categoryService;
        }

        // GET: api/Categories
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CategoriesDTO>>> GetCategory()
        {

            return await _categoryService.GetCategory();
        }

        // GET: api/Categories/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CategoriesDTO>> GetCategory(string id)
        {
            if(string.IsNullOrWhiteSpace(id))
            { return BadRequest(); }

            var category = await _categoryService.GetCategory(id);

            if (category == null)
            {
                return NotFound();
            }

            return category;
        }

        // PUT: api/Categories/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCategory(string id, CategoryPutDTO categoryDTO)
        {
            if (String.IsNullOrWhiteSpace(id) || categoryDTO == null)
            {
                return BadRequest();
            }
            var category = await _categoryService.GetCategory(id);
            if (category == null)
            {
                return NotFound();
            }
            await _categoryService.PutCategory(id, categoryDTO);

            return Ok();
        }

        // POST: api/Categories
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<CategoryPostDTO>> PostCategory([FromBody]CategoryPostDTO category)
        {
            if(_categoryService.CategoryExists(category.CateID))
            {
                return BadRequest("Category already exists.");
            }

            Category cate = await _categoryService.PostCategory(category);

            return CreatedAtAction("GetCategory", new { id = category.CateID }, category);
        }

        // DELETE: api/Categories/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return BadRequest();
            }
            var category = await _categoryService.GetCategory(id);
            if (category == null)
            {
                return NotFound();
            }
            bool success = await _categoryService.DeleteCategory(id);
            if (success) {
                return Ok();
            }
            return BadRequest();
        }


    }
}
