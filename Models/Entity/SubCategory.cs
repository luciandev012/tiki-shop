namespace tiki_shop.Models.Entity
{
    public class SubCategory
    {
        public string? Id { get; set; }
        public string? Name { get; set; }
        public string? Image { get; set; }
        public ICollection<Product>? Products { get; set; }
        public string? CategoryId { get; set; }
        public Category? Category { get; set; }
    }
}
