using Data.Interfaces;
using Data.Models;
using Data.Repositories;
using Domain.Exceptions;
using Domain.Interfaces;
using Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace Domain.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly ILogbookRepository _logbookRepository;
        private readonly IDistrictRepository _districtRepository;
        private readonly int _minutes;
        private readonly int _decimalPlacesNum;

        public OrderService(IOrderRepository orderRepository,
                            ILogbookRepository logbookRepository,
                            IDistrictRepository districtRepository,
                            IOptions<DeliveryConfigurationOptions> options)
        {
            _orderRepository = orderRepository ?? throw new ArgumentNullException(nameof(orderRepository));
            _logbookRepository = logbookRepository ?? throw new ArgumentNullException(nameof(logbookRepository));
            _districtRepository = districtRepository ?? throw new ArgumentNullException(nameof(districtRepository));

            _minutes = options.Value.OrderOptions.MinutesAfterFirst;
            _decimalPlacesNum = options.Value.OrderOptions.DecimalPlacesNum;
        }

        public async Task<Order> CreateOrderAsync(Order newOrder, IPAddress ip)
        {
            using var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

            newOrder.Weight = Math.Round(newOrder.Weight, _decimalPlacesNum);
            
            do
            {
                newOrder.Number = GenerateRandomNumber();

            } while (await _orderRepository.GetOrderByOrderNumberAsync(newOrder.Number).ConfigureAwait(false) is not null);

            await _orderRepository.CreateOrderAsync(newOrder).ConfigureAwait(false);

            var logbook = new Logbook()
            {
                AccessAt = DateTime.Now,
                IpAddress = ip.ToString()
            };

            await _logbookRepository.CreateLogbookAsync(logbook).ConfigureAwait(false);

            transaction.Complete();
            transaction.Dispose();

            return await _orderRepository.GetOrderByIdAsync(newOrder.Id).ConfigureAwait(false) ?? throw new OrderNotFoundException();
        }

        public async Task<IEnumerable<Order>> GetOrdersByDistrictAsync(Guid districtId, DateTimeOffset firstDeliveryDateTime, int page, int perPage)
        {
            _ = await _districtRepository.GetDistrictByIdAsync(districtId).ConfigureAwait(false) ?? throw new DistrictNotFoundException();
            _ = await _orderRepository.GetOrderByFirstDeliveryDateTimeAsync(firstDeliveryDateTime.DateTime, districtId).ConfigureAwait(false) ?? throw new OrderNotFoundException();

            return await _orderRepository.GetOrdersByDistrictAndTimeAsync(districtId, firstDeliveryDateTime.DateTime, _minutes, page, perPage).ConfigureAwait(false);
        }

        public async Task<MemoryStream> GetOrdersByDistrictAsync(Guid districtId, DateTimeOffset firstDeliveryDateTime)
        {
            _ = await _districtRepository.GetDistrictByIdAsync(districtId).ConfigureAwait(false) ?? throw new DistrictNotFoundException();
            _ = await _orderRepository.GetOrderByFirstDeliveryDateTimeAsync(firstDeliveryDateTime.DateTime, districtId).ConfigureAwait(false) ?? throw new OrderNotFoundException();

            var orders = await _orderRepository.GetOrdersByDistrictAndTimeAsync(districtId, firstDeliveryDateTime.DateTime, _minutes, null, null).ConfigureAwait(false);

            return WriteOrdersToMemoryStream(orders);
        }

        private static string GenerateRandomNumber()
        {
            int size = 5;
            string a = "1234567890";
            StringBuilder result = new(size);
            using var rng = new RNGCryptoServiceProvider();
            while (result.Length < size)
            {
                var bytes = new byte[1];
                rng.GetBytes(bytes);
                if (bytes[0] >= (byte)(a.Length - 1)) continue;
                result.Append(a[bytes[0]]);
            }
            return result.ToString();
        }

        private static MemoryStream WriteOrdersToMemoryStream(IEnumerable<Order> orders)
        {
            var memoryStream = new MemoryStream();
            using (var writer = new StreamWriter(memoryStream, Encoding.UTF8, leaveOpen: true))
            {
                foreach (var order in orders)
                {
                    writer.WriteLine($"Id: {order.Id}, " +
                        $"Number: {order.Number}, " +
                        $"Weight: {order.Weight}, " +
                        $"DeliveryTime: {order.DeliveryTime.ToString("yyyy-MM-dd HH:mm:ss")}, " +
                        $"DistrictId: {order.DistrictId}");
                }
                writer.Flush();
            }
            return memoryStream;
        }
    }
}
