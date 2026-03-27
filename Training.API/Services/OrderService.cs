using System.Net;
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
            
            var orderItems = new List<OrderItem>();
            decimal totalAmount = 0;
            int userId = 0;
            
           
            foreach (var item in request.Items)
            {
                var createBookDetails = item.CreateBookDetails;

                int bookId = 0, quantity = 0;
                foreach (var detail in createBookDetails)
                {
                    bookId = detail.BookId;
                    quantity = detail.Quantity;
                }


                var book = await _bookRepository.GetByIdAsync(bookId);
                
                // 判斷書及是否存在
                if (book == null)
                    throw new BookNotFoundException(bookId);

                //  檢查庫存
                if (book.Stock < quantity)
                    throw new InsufficientStockException(book.Title);

                userId = item.UserId;

                // 扣庫存
                book.Stock -= quantity;

                var subTotal = book.Price * quantity;
                totalAmount += subTotal;

                //  建立明細
                orderItems.Add(new OrderItem
                {
                    BookId = book.Id,
                    Quantity = quantity,
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
            
        }
    }
} 
