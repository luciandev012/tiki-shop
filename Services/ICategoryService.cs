using tiki_shop.Models.Common;
using tiki_shop.Models.Entity;
using tiki_shop.Models.Request.Category;

namespace tiki_shop.Services
{
    public interface ICategoryService
    {
        Task<ResultList<Category>> GetAllCategories();
        Task<Result<Category>> AddCategory(string name);
        Task<Result<SubCategory>> AddSubCategory(SubCategoryRequest req);
    }
}
