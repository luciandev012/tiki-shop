namespace tiki_shop.Models.Request.Category
{
    public class SubCategoryRequest
    {
        public string Name { get; set; }
        public string CategoryId { get; set; }
        public IFormFile Image { get; set; }
    }
}
