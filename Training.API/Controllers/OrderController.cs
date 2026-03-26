using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Training.API.Services;
using Training.Core.DTOs;
using Training.Core.interfaces;
using Training.Core.Models;

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
    }
}
