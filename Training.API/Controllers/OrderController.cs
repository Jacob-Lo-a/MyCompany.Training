using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NPOI.HPSF;
using Training.Core.DTOs;
using Training.Core.interfaces;

namespace Training.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;
        private readonly ISftpService _sftpService;

        public OrderController(IOrderService orderService, ISftpService sftpService)
        {
            _orderService = orderService;
            _sftpService = sftpService;
        }

        [Authorize(Roles = "User")]
        [HttpPost]
        public async Task<IActionResult> CreateOrder(CreateOrderRequest request)
        {
           

            await _orderService.CreateOrderAsync(request);

            return Ok(new
            {
                message = "訂單建立成功"
            });
        }

        [HttpGet("export")]
        public async Task<IActionResult> Export()
        {
            var fileBytes = await _orderService.ExportOrdersAsync();

            return File(
                fileBytes,
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                "OrdersReport.xlsx"
            );
        }

        [HttpPost("send-report")]
        public async Task<IActionResult> SendReport()
        {
            var fileBytes = await _orderService.ExportOrdersAsync();

            var fileName = $"OrderReport_{DateTime.Now:yyyyMMddHHmmss}.xlsx";

            await _sftpService.UploadReportAsync(fileBytes, fileName);

            return Ok(new { message = "檔案已成功上傳" });
        }

        [HttpPost("upload-File")]
        public async Task<IActionResult> UploadFile(IFormFile file)
        {
            await _sftpService.UploadFileAsync(file);
            return Ok(new 
            { 
                message = "檔案已成功上傳",
                FileName = file.FileName
            });
        }
    }
}
