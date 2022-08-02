namespace tiki_shop.Models.Request.Product
{
    public class ProductRequest
    {
        public string? Id { get; set; }
        public string? Name { get; set; }
        public float Price { get; set; }
        public float Commission { get; set; }
        public int Quantity { get; set; }
        public string? Description { get; set; }
        public string? SubCategoryId { get; set; }
        public IFormFile? Image { get; set; }
    }
}
