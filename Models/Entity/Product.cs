namespace tiki_shop.Models.Entity
{
    public class Product
    {
        public string? Id { get; set; }
        public string? Name { get; set; }
        public string? Slug { get; set; }
        public float Price { get; set; }
        public float Commission { get; set; }
        public int Quantity { get; set; }
        public float Discount { get; set; }
        public int SoldQuantity { get; set; } = 0;
        public ICollection<Image>? Images { get; set; }
        public ICollection<ProductDetail>? ProductDetails { get; set; }
        public string? Description { get; set; }
        public float Rate { get; set; } = 5;
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string? SubCategoryId { get; set; }
        public SubCategory? SubCategory { get; set; }
        
    }
}
