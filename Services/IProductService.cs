using tiki_shop.Models.Common;
using tiki_shop.Models.DTO;
using tiki_shop.Models.Request.Product;

namespace tiki_shop.Services
{
    public interface IProductService
    {
        Task<ResultList<ProductDTO>> GetAllProducts();
        Task<Result<ProductDTO>> AddProduct(ProductRequest req);
    }
}
