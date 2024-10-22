using Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Interfaces
{
    public interface IOrderRepository
    {
        public Task CreateOrderAsync(Order order);
        public Task<Order?> GetOrderByIdAsync(Guid id);
        public Task<Order?> GetOrderByFirstDeliveryDateTimeAsync(DateTime firstDeliveryDateTime, Guid districtId);
        public Task<IEnumerable<Order>> GetOrdersByDistrictAndTimeAsync(Guid districtId, DateTime firstDeliveryDateTime, int minutes, int? page, int? perPage);
        public Task<Order?> GetOrderByOrderNumberAsync(string number);
    }
}
