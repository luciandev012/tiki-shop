using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System.Text.Json.Serialization;
using tiki_shop.Models.Request.Product;
using tiki_shop.Services;

namespace tiki_shop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;
        public ProductController(IProductService productService)
        {
            _productService = productService;
        }
        [HttpGet("getall")]
        public async Task<IActionResult> GetAllProducts()
        {
            var res = await _productService.GetAllProducts();
            if (!res.Success)
            {
                return BadRequest(res);
            }
            JsonSerializerOptions options = new()
            {
                ReferenceHandler = ReferenceHandler.Preserve,
                WriteIndented = true
            };
            var data = JsonSerializer.Serialize(res, options);
            return new OkObjectResult(data);
        }
        [HttpPost("add")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddProduct([FromBody] ProductRequest req)
        {
            var res = await _productService.AddProduct(req);
            if (!res.Success)
            {
                return BadRequest(res);
            }
            JsonSerializerOptions options = new()
            {
                ReferenceHandler = ReferenceHandler.Preserve,
                WriteIndented = true
            };
            var data = JsonSerializer.Serialize(res, options);
            return new OkObjectResult(data);
        }
    }
}
