using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System.Text.Json.Serialization;
using tiki_shop.Models.Request.Product;
using tiki_shop.Services;
using tiki_shop.Services.Common;

namespace tiki_shop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;
        private readonly IStorageService _storage;
        public ProductController(IProductService productService, IStorageService storage)
        {
            _productService = productService;
            _storage = storage;
        }
        [HttpGet("getall")]
        public async Task<IActionResult> GetAllProducts()
        {
            var res = await _productService.GetAllProducts();
            if (!res.Success)
            {
                return BadRequest(res);
            }
            //JsonSerializerOptions options = new()
            //{
            //    ReferenceHandler = ReferenceHandler.Preserve,
            //    WriteIndented = true
            //};
            var data = JsonSerializer.Serialize(res);
            return new OkObjectResult(data);
        }
        [HttpPost("add")]
        [Consumes("multipart/form-data")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddProduct([FromForm] ProductRequest req)
        {
            var res = await _productService.AddProduct(req);
            if (!res.Success)
            {
                return BadRequest(res);
            }
            //JsonSerializerOptions options = new()
            //{
            //    ReferenceHandler = ReferenceHandler.Preserve,
            //    WriteIndented = true
            //};
            var data = JsonSerializer.Serialize(res);
            return new OkObjectResult(data);
        }
        [HttpGet("image/{name}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetImage([FromRoute] string name)
        {
            var filePath = _storage.GetPath(name);
            if(System.IO.File.Exists(filePath))
            {
                byte[] b = System.IO.File.ReadAllBytes(filePath);
                return File(b, "image/png");
            }
            return null;
        }
    }
}
