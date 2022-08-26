using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using tiki_shop.Models;
using tiki_shop.Models.Common;
using tiki_shop.Models.Entity;
using tiki_shop.Models.Request.Category;
using tiki_shop.Services.Common;

namespace tiki_shop.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly IMongoCollection<Category> _categoryCollection;
        private readonly IStorageService _storage;
        public CategoryService(IOptions<TikiDbSettings> tikiDb, IStorageService storage)
        {
            var mongoClient = new MongoClient(tikiDb.Value.ConnectionString);
            var mongoDb = mongoClient.GetDatabase(tikiDb.Value.DatabaseName);
            _categoryCollection = mongoDb.GetCollection<Category>("categories");
            _storage = storage;
        }

        public async Task<Result<Category>> AddCategory(CategoryRequest req)
        {
            try
            {
                var category = new Category
                {
                    Name = req.Name,
                    Image = await _storage.SaveFileAsync(req.Image),
                    SubCategories = new SubCategory[] {}
                };
                await _categoryCollection.InsertOneAsync(category);
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
                var filter = Builders<Category>.Filter.Eq(c => c.Id, req.CategoryId);
                var subCate = new SubCategory
                {
                    Id = ObjectId.GenerateNewId().ToString(),
                    Name = req.Name,
                    Image = await _storage.SaveFileAsync(req.Image),
                };
                var update = Builders<Category>.Update.Push<SubCategory>(s => s.SubCategories, subCate);
                var result = await _categoryCollection.FindOneAndUpdateAsync(filter, update);
                return new Result<SubCategory> { Data = subCate, Success = true };
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
                var categories = await _categoryCollection.Find(_ => true).ToListAsync();
                return new ResultList<Category> { Data = categories, Success = true };
            }
            catch (Exception)
            {

                return new ResultList<Category> { Success = false, Message = "Server error" };
            }
        }
        
    }
}
