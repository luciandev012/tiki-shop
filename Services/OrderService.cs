using tiki_shop.Models;
using tiki_shop.Models.Common;
using tiki_shop.Models.DTO;
using tiki_shop.Models.Request.Order;

namespace tiki_shop.Services
{
    public class OrderService : IOrderService
    {
        private readonly TikiDbContext _context;
        public OrderService(TikiDbContext context)
        {
            _context = context;
        }

        public Task<Result<OrderDTO>> AddOrder(OrderRequest req)
        {
            throw new NotImplementedException();
        }
        //public Task<Result<OrderDTO>> AddOrder(OrderRequest req)
        //{
        //    try
        //    {
        //        if(req.OrderDetails.Count > 0)
        //        {
        //            float total, commission = 0;
        //            foreach(var order in req.OrderDetails)
        //            {
        //                var 
        //            }
        //        }
        //    }
        //    catch (Exception)
        //    {

        //        throw;
        //    }
        //}
    }
}
