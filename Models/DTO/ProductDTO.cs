using tiki_shop.Models.Entity;

namespace tiki_shop.Models.DTO
{
    public class ProductDTO
    {
        public string? Id { get; set; }
        public string? Name { get; set; }
        public string? Slug { get; set; }
        public float Price { get; set; }
        public float Commission { get; set; }
        public int Quantity { get; set; }
        public float Discount { get; set; }
        public int SoldQuantity { get; set; } = 0;
        public List<Image>? Images { get; set; }
        public List<ProductDetail>? ProductDetails { get; set; }
        public string? Description { get; set; }
        public float Rate { get; set; } = 5;
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string? SubCategory { get; set; }
    }
}
