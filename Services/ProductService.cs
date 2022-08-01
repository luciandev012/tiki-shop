using Microsoft.EntityFrameworkCore;
using tiki_shop.Models;
using tiki_shop.Models.Common;
using tiki_shop.Models.DTO;
using tiki_shop.Models.Entity;
using tiki_shop.Models.Request.Product;

namespace tiki_shop.Services
{
    public class ProductService : IProductService
    {
        private readonly TikiDbContext _context;
        public ProductService(TikiDbContext context)
        {
            _context = context;
        }

        public async Task<Result<ProductDTO>> AddProduct(ProductRequest req)
        {
            try
            {
                var product = new Product
                {
                    Id = Guid.NewGuid().ToString(),
                    Commission = req.Commission,
                    CreatedDate = DateTime.Now,
                    Description = req.Description,
                    Name = req.Name,
                    Price = req.Price,
                    Quantity = req.Quantity,
                    SubCategoryId = req.SubCategoryId

                };
                await _context.Products.AddAsync(product);
                await _context.SaveChangesAsync();
                var productDTO = new ProductDTO
                {
                    Id = product.Id, Commission = product.Commission, CreatedDate = product.CreatedDate,
                    Description = product.Description, Discount = product.Discount,
                    Name = product.Name, Price = product.Price, Rate = product.Rate,
                    Slug = product.Slug, Quantity = product.Quantity, SoldQuantity = product.SoldQuantity,
                    ModifiedDate = product.ModifiedDate, SubCategory = (await _context.SubCategories.FindAsync(product.SubCategoryId)).Name,
                };
                return new Result<ProductDTO> { Data = productDTO, Success = true };
            }
            catch (Exception)
            {

                return new Result<ProductDTO> { Success = false, Message = "Server error" };
            }
        }

        public async Task<ResultList<ProductDTO>> GetAllProducts()
        {
            try
            {
                var res = await _context.Products.ToListAsync();
                var products = new List<ProductDTO>();
                foreach (var product in res)
                {
                    var dto = new ProductDTO
                    {
                        Id = product.Id,
                        Commission = product.Commission,
                        CreatedDate = product.CreatedDate,
                        Description = product.Description,
                        Discount = product.Discount,
                        Name = product.Name,
                        Price = product.Price,
                        Rate = product.Rate,
                        Slug = product.Slug,
                        Quantity = product.Quantity,
                        SoldQuantity = product.SoldQuantity,
                        ModifiedDate = product.ModifiedDate,
                        SubCategory = (await _context.SubCategories.FindAsync(product.SubCategoryId)).Name,
                    };
                    products.Add(dto);
                }
                return new ResultList<ProductDTO> { Data = products, Success = true };
            }
            catch (Exception)
            {

                return new ResultList<ProductDTO> { Success = false, Message = "Server error" };
            }
        }
    }
}
