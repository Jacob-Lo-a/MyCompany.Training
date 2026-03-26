using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Training.Core.DTOs;

namespace Training.Core.interfaces
{
    public interface IOrderService
    {
        Task CreateOrderAsync(CreateOrderRequest request);
    }
}
