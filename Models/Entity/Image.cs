namespace tiki_shop.Models.Entity
{
    public class Image
    {
        public string? Id { get; set; }
        public string? Url { get; set; }
        public string? ProductId { get; set; }
        public Product? Product { get; set; }
    }
}
