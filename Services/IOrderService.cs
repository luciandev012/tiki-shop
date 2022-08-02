using tiki_shop.Models.Common;
using tiki_shop.Models.DTO;
using tiki_shop.Models.Request.Order;

namespace tiki_shop.Services
{
    public interface IOrderService
    {
        Task<Result<OrderDTO>> AddOrder(OrderRequest req);
        Task<ResultList<OrderDTO>> GetAllOrder();
        Task<ResultList<OrderDTO>> GetAllOrderByUser();
    }
}
