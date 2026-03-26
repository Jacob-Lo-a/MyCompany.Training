using Training.API.Repositories;
using Training.Core.DTOs;
using Training.Core.interfaces;
using Training.Core.Models;
using Training.Core.Repositories;

namespace Training.API.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IBookRepository _bookRepository;
        
        public OrderService(IOrderRepository orderRepository, IBookRepository bookRepository)
        {
            _orderRepository = orderRepository;
            _bookRepository = bookRepository;
        }
        public async Task CreateOrderAsync(CreateOrderRequest request)
        {
            using var transaction = await _orderRepository.BeginTransactionAsync();
            
            try
            {
                var orderItems = new List<OrderItem>();
                decimal totalAmount = 0;
                int userId = 0;

                foreach (var item in request.Items)
                {
                    var book = await _bookRepository.GetByIdAsync(item.BookId);

                    if (book == null)
                        throw new Exception($"書籍不存在 (ID: {item.BookId})");

                    // 設定 userId
                    userId = item.UserId;

                    //  檢查庫存
                    if (book.Stock < item.Quantity)
                        throw new InsufficientStockException(book.Title);

                    // 扣庫存
                    book.Stock -= item.Quantity;

                    var subTotal = book.Price * item.Quantity;
                    totalAmount += subTotal;

                    //  建立明細
                    orderItems.Add(new OrderItem
                    {
                        BookId = book.Id,
                        Quantity = item.Quantity,
                        UnitPrice = book.Price
                    });
                }

                //  建立訂單
                var order = new Order
                {
                    OrderNumber = Guid.NewGuid().ToString(),
                    UserId = userId,
                    TotalAmount = totalAmount,
                    Status = "Pending",
                    CreatedAt = DateTime.UtcNow,
                    OrderItems = orderItems
                };
                
                await _orderRepository.AddAsync(order);
                await _orderRepository.SaveChangesAsync();
                
                await transaction.CommitAsync();
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }
    }
} 
