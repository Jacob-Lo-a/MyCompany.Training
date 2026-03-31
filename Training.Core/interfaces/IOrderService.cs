using Training.Core.DTOs;

namespace Training.Core.interfaces
{
    public interface IOrderService
    {
        Task CreateOrderAsync(CreateOrderRequest request);
        Task<byte[]> ExportOrdersAsync();
    }
}
