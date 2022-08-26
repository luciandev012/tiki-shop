using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System.Text.Json.Serialization;
using tiki_shop.Models.Request.Category;
using tiki_shop.Services;

namespace tiki_shop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryServices;
        public CategoryController(ICategoryService categoryServices)
        {
            _categoryServices = categoryServices;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllCategories()
        {
            var res = await _categoryServices.GetAllCategories();
            if (!res.Success)
            {
                return BadRequest(res);
            }
            var data = JsonSerializer.Serialize(res);
            return Ok(data);
        }
        [HttpPost("addCategory")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddCategory([FromForm] CategoryRequest req)
        {
            var res = await _categoryServices.AddCategory(req);
            if (!res.Success)
            {
                return BadRequest(res);
            }
            return Ok(res);
        }
        [HttpPost("addSubcate")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddSubCategory([FromForm] SubCategoryRequest req)
        {
            var res = await _categoryServices.AddSubCategory(req);
            if (!res.Success)
            {
                return BadRequest(res);
            }
            return Ok(res);
        }
    }
}
