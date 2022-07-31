namespace tiki_shop.Models.Entity
{
    public class ProductDetail
    {
        public string? Id { get; set; }
        public string? Name { get; set; }
        public string? Value { get; set; }
        public string? ProductId { get; set; }
        public Product? Product { get; set; }

    }
}
