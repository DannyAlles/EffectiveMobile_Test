using Data.Interfaces;
using Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly DeliveryDbContext _context;

        public OrderRepository(DeliveryDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task CreateOrderAsync(Order order)
        {
            await _context.AddAsync(order).ConfigureAwait(false);
            await _context.SaveChangesAsync().ConfigureAwait(false);
        }

        public async Task<Order?> GetOrderByFirstDeliveryDateTimeAsync(DateTime firstDeliveryDateTime, Guid districtId)
        {
            return await _context.Orders.AsNoTracking().FirstOrDefaultAsync(x => x.DeliveryTime == firstDeliveryDateTime && x.DistrictId == districtId).ConfigureAwait(false);
        }

        public async Task<Order?> GetOrderByIdAsync(Guid id)
        {
            return await _context.Orders.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id).ConfigureAwait(false);
        }
        
        public async Task<Order?> GetOrderByOrderNumberAsync(string number)
        {
            return await _context.Orders.AsNoTracking().FirstOrDefaultAsync(x => x.Number == number).ConfigureAwait(false);
        }

        public async Task<IEnumerable<Order>> GetOrdersByDistrictAndTimeAsync(Guid districtId, DateTime firstDeliveryDateTime, int minutes, int? page, int? perPage)
        {
            var query =  _context.Orders.AsNoTracking()
                                        .Include(x => x.District)
                                        .Where(x => x.DistrictId == districtId &&
                                                    x.DeliveryTime >= firstDeliveryDateTime &&
                                                    x.DeliveryTime <= firstDeliveryDateTime.AddMinutes(minutes))
                                        .OrderBy(x => x.DeliveryTime);

            if (page is not null && perPage is not null)
            {
                query.Skip((page.Value - 1) * perPage.Value)
                     .Take(perPage.Value);
            }

            return await query.ToListAsync()
                              .ConfigureAwait(false);
        }
    }
}
