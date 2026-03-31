using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Training.Core.DTOs;
using Training.Core.interfaces;

namespace Training.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
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
    }
}
