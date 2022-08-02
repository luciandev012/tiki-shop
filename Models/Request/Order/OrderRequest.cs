using tiki_shop.Models.Entity;

namespace tiki_shop.Models.Request.Order
{
    public class OrderRequest
    {
        public List<OrderDetail> OrderDetails { get; set; }
    }
}
