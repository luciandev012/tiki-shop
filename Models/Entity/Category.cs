namespace tiki_shop.Models.Entity
{
    public class Category
    {
        public string? Id { get; set; }
        public string? Name { get; set; }
        public string? Image { get; set; }
        public ICollection<SubCategory>? SubCategories { get; set; }
    }
}
