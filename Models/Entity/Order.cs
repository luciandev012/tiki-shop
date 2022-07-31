namespace tiki_shop.Models.Entity
{
    public class Order
    {
        public string? Id { get; set; }
        public string? UserId { get; set; }
        public DateTime? CreatedDate { get; set; }
        public User? User { get; set; }
        public float TotalPrice { get; set; }
        public bool Status { get; set; }
        public float TotalCommission { get; set; }
        public ICollection<OrderDetail>? OrderDetails { get; set; }
    }
}
