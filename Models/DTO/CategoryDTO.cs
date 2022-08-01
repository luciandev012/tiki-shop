using tiki_shop.Models.Entity;

namespace tiki_shop.Models.DTO
{
    public class CategoryDTO
    {
        public string? Id { get; set; }
        public string? Name { get; set; }
        public string? Image { get; set; }
        public List<SubCategory>? SubCategories { get; set; }
    }
}
