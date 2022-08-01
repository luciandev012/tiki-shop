using Microsoft.EntityFrameworkCore;
using tiki_shop.Models;
using tiki_shop.Models.Common;
using tiki_shop.Models.Entity;
using tiki_shop.Models.Request.Category;

namespace tiki_shop.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly TikiDbContext _context;
        public CategoryService(TikiDbContext context)
        {
            _context = context;
        }

        public async Task<Result<Category>> AddCategory(string name)
        {
            try
            {
                var category = new Category
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = name
                };
                await _context.Categories.AddAsync(category);
                await _context.SaveChangesAsync();
                return new Result<Category> { Data = category, Success = true };
            }
            catch (Exception)
            {
                return new Result<Category> { Success = false, Message = "Server error" };
            }
        }

        public async Task<Result<SubCategory>> AddSubCategory(SubCategoryRequest req)
        {
            try
            {
                var subCate = new SubCategory
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = req.Name,
                    CategoryId = req.CategoryId
                };
                await _context.SubCategories.AddAsync(subCate);
                await _context.SaveChangesAsync();
                return new Result<SubCategory> { Data= subCate, Success = true };
            }
            catch (Exception)
            {

                return new Result<SubCategory> { Success = false, Message = "Server error" };
            }
        }

        public async Task<ResultList<Category>> GetAllCategories()
        {
            try
            {
                var categories = await _context.Categories.ToListAsync();      
                for(int i = 0; i < categories.Count; i++)
                {
                    categories[i].SubCategories = await _context.SubCategories.Where(x => x.CategoryId == categories[i].Id).ToListAsync();
                }
                return new ResultList<Category> { Data = categories, Success = true };
            }
            catch (Exception)
            {

                return new ResultList<Category> { Success = false, Message = "Server error" };
            }
        }
        
    }
}
