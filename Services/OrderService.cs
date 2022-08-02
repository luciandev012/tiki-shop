using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using tiki_shop.Models;
using tiki_shop.Models.Common;
using tiki_shop.Models.DTO;
using tiki_shop.Models.Entity;
using tiki_shop.Models.Request.Order;

namespace tiki_shop.Services
{
    public class OrderService : IOrderService
    {
        private readonly TikiDbContext _context;
        private readonly IHttpContextAccessor _contextAccessor;
        public OrderService(TikiDbContext context, IHttpContextAccessor contextAccessor)
        {
            _context = context;
            _contextAccessor = contextAccessor;
        }

        public async Task<Result<OrderDTO>> AddOrder(OrderRequest req)
        {
            try
            {
                if (req.OrderDetails.Count > 0)
                {
                    float total = 0, commission = 0;
                    ClaimsPrincipal User = _contextAccessor.HttpContext.User;
                    var id = User.FindFirstValue(ClaimTypes.NameIdentifier);
                    var user = await _context.Users.FindAsync(id);
                    foreach (var orderDetail in req.OrderDetails)
                    {
                        var product = await _context.Products.FindAsync(orderDetail.ProductId);
                        total += product.Price * orderDetail.OrderQuantity - product.Price * orderDetail.OrderQuantity * product.Discount;
                        commission += product.Commission * orderDetail.OrderQuantity;
                        orderDetail.Id = Guid.NewGuid().ToString();
                    }
                    if(user.Balance - total >= 0)
                    {
                        var order = new Order
                        {
                            Id = Guid.NewGuid().ToString(),
                            CreatedDate = DateTime.Now,
                            OrderDetails = req.OrderDetails,
                            Status = true,
                            TotalCommission = commission,
                            TotalPrice = total,
                            UserId = id
                        };
                        await _context.Orders.AddAsync(order);
                        user.Balance -= total;
                        user.Commission += commission;
                        await _context.SaveChangesAsync();
                        var orderDTO = new OrderDTO
                        {
                            OrderDetails = order.OrderDetails.ToList(),
                            Status = order.Status,
                            TotalCommission = order.TotalCommission,
                            TotalPrice = order.TotalPrice,
                            UserId = order.UserId,
                            Id = order.Id
                        };
                        return new Result<OrderDTO> { Data = orderDTO, Success = true };
                    }
                    else
                    {
                        return new Result<OrderDTO> { Message = "Your balance is not enough", Success = false };
                    }    
                }
                return new Result<OrderDTO> { Message = "Your balance is not enough", Success = false };
            }
            catch (Exception)
            {

                return new Result<OrderDTO> { Success = false, Message = "Server error" };
            }
        }

        public async Task<ResultList<OrderDTO>> GetAllOrder()
        {
            try
            {
                var orders = await _context.Orders.ToListAsync();
                var orderDTOs = new List<OrderDTO>();
                foreach (var order in orders)
                {
                    var orderDTO = new OrderDTO
                    {
                        Id = order.Id,
                        OrderDetails = order.OrderDetails.ToList(),
                        Status = order.Status,
                        TotalCommission = order.TotalCommission,
                        TotalPrice = order.TotalPrice,
                        UserId = order.UserId
                    };
                    orderDTOs.Add(orderDTO);
                }
                return new ResultList<OrderDTO> { Data = orderDTOs, Success = true };
            }
            catch (Exception)
            {
                return new ResultList<OrderDTO> { Success = false, Message = "Server error" };

            }
        }

        public async Task<ResultList<OrderDTO>> GetAllOrderByUser()
        {
            try
            {
                ClaimsPrincipal User = _contextAccessor.HttpContext.User;
                var id = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var orders = await _context.Orders.Where(x => x.UserId == id).ToListAsync();
                var orderDTOs = new List<OrderDTO>();
                foreach (var order in orders)
                {
                    var orderDTO = new OrderDTO
                    {
                        Id = order.Id,
                        OrderDetails = order.OrderDetails.ToList(),
                        Status = order.Status,
                        TotalCommission = order.TotalCommission,
                        TotalPrice = order.TotalPrice,
                        UserId = order.UserId
                    };
                    orderDTOs.Add(orderDTO);
                }
                return new ResultList<OrderDTO> { Data = orderDTOs, Success = true };
            }
            catch (Exception)
            {

                return new ResultList<OrderDTO> { Success = false, Message = "Server error" };
            }
        }
    }
}
