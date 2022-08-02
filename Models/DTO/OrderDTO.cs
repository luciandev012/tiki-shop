using tiki_shop.Models.Entity;

namespace tiki_shop.Models.DTO
{
    public class OrderDTO
    {
        public string? UserId { get; set; }
        public float TotalPrice { get; set; }
        public bool Status { get; set; }
        public float TotalCommission { get; set; }
        public List<OrderDetail>? OrderDetails { get; set; }
    }
}
