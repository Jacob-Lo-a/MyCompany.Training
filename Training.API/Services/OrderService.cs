using Hangfire;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using Training.API.Exceptions;
using Training.Core.DTOs;
using Training.Core.interfaces;
using Training.Core.Models;

namespace Training.API.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IBookRepository _bookRepository;
        private readonly ISftpService _sftpService;

        public OrderService(IOrderRepository orderRepository, IBookRepository bookRepository, ISftpService sftpService)
        {
            _orderRepository = orderRepository;
            _bookRepository = bookRepository;
            _sftpService = sftpService;
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

            BackgroundJob.Enqueue<EmailService>(
                x => x.SendOrderEmailAsync(order.Id)
            );
        }

        public async Task<byte[]> ExportOrdersAsync()
        {
            var orders = await _orderRepository.GetAllWithUserAsync();

            IWorkbook workbook = new XSSFWorkbook();
            ISheet sheet = workbook.CreateSheet("訂單報表");

            IRow headerRow = sheet.CreateRow(0);
            headerRow.CreateCell(0).SetCellValue("訂單編號");
            headerRow.CreateCell(1).SetCellValue("總金額");
            headerRow.CreateCell(2).SetCellValue("購買人帳號");
            headerRow.CreateCell(3).SetCellValue("建立時間");

            int rowIndex = 1;

            foreach (var order in orders)
            {
                IRow row = sheet.CreateRow(rowIndex);

                row.CreateCell(0).SetCellValue(order.OrderNumber);
                row.CreateCell(1).SetCellValue((double)order.TotalAmount);
                row.CreateCell(2).SetCellValue(order.User?.Account ?? "");
                row.CreateCell(3).SetCellValue(
                    order.CreatedAt.ToString("yyyy-MM-dd HH:mm:ss")
                );

                rowIndex++;
            }

            // 調整欄位大小
            for (int i = 0; i < 4; i++)
            {
                sheet.AutoSizeColumn(i);
            }

            using var ms = new MemoryStream();
            workbook.Write(ms);

            return ms.ToArray();
        }

        public async Task SyncDailySalesReportAsync()
        {
            var today = DateTime.Today;
            var yesterdayStart = today.AddDays(-1); // 昨天 00:00:00
            var yesterdayEnd = today;               // 今天 00:00:00

            // 撈取昨天訂單
            var orders = await _orderRepository.GetByDateRangeAsync(yesterdayStart, yesterdayEnd);

            IWorkbook workbook = new XSSFWorkbook();
            ISheet sheet = workbook.CreateSheet("訂單報表");

            IRow headerRow = sheet.CreateRow(0);
            headerRow.CreateCell(0).SetCellValue("訂單編號");
            headerRow.CreateCell(1).SetCellValue("總金額");
            headerRow.CreateCell(2).SetCellValue("購買人帳號");
            headerRow.CreateCell(3).SetCellValue("建立時間");

            int rowIndex = 1;

            foreach (var order in orders)
            {
                IRow row = sheet.CreateRow(rowIndex);

                row.CreateCell(0).SetCellValue(order.OrderNumber);
                row.CreateCell(1).SetCellValue((double)order.TotalAmount);
                row.CreateCell(2).SetCellValue(order.User?.Account ?? "");
                row.CreateCell(3).SetCellValue(
                    order.CreatedAt.ToString("yyyy-MM-dd HH:mm:ss")
                );

                rowIndex++;
            }

            // 調整欄位大小
            for (int i = 0; i < 4; i++)
            {
                sheet.AutoSizeColumn(i);
            }

            using var ms = new MemoryStream();
            workbook.Write(ms);

            var fileBytes = ms.ToArray();

            var fileName = $"DailySales_{DateTime.Now:yyyyMMdd}.xlsx";

            await _sftpService.UploadReportAsync(fileBytes, fileName);
        }
    }
}