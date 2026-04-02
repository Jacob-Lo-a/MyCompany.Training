using Training.Core.interfaces;

namespace Training.API.Services
{
    public class EmailService : IEmailService
    {
        public async Task SendOrderEmailAsync(int orderId)
        {
            Console.WriteLine($"開始寄信：Order {orderId}");

            await Task.Delay(3000);

            Console.WriteLine($"寄信完成：Order {orderId}");
        }
    }
}
