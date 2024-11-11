using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IOrderService
    {
        public Task<Order> CreateOrderAsync(Order order, IPAddress ip);
        public Task<IEnumerable<Order>> GetOrdersByDistrictAsync(Guid districtId, DateTimeOffset firstDeliveryDateTime, int page, int perPage);
        public Task<MemoryStream> GetOrdersByDistrictAsync(Guid districtId, DateTimeOffset firstDeliveryDateTime);
    }
}
