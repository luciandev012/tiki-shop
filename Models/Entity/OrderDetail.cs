namespace tiki_shop.Models.Entity
{
    public class OrderDetail
    {
        public string? Id { get; set; }
        public string? ProductId { get; set; }
        public string? OrderId { get; set; }
        public Order? Order { get; set; }
    }
}
