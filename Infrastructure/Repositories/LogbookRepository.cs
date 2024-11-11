using Domain.Interfaces;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class LogbookRepository : ILogbookRepository
    {
        private readonly DeliveryDbContext _context;

        public LogbookRepository(DeliveryDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task CreateLogbookAsync(Logbook logbook)
        {
            await _context.Logbooks.AddAsync(logbook).ConfigureAwait(false);
            await _context.SaveChangesAsync().ConfigureAwait(false);
        }

        public async Task<IEnumerable<Logbook>> GetLogbooksByIpAsync(IPAddress startAddress, IPAddress endAddress, DateTime startDate, DateTime endDate)
        {
            var logbooks = await _context.Logbooks.AsNoTracking()
                .Where(x => x.AccessAt >= startDate && x.AccessAt <= endDate).ToListAsync().ConfigureAwait(false);

            return logbooks.Where(x => IsIpInRange(IPAddress.Parse(x.IpAddress), startAddress, endAddress));
        }

        private static bool IsIpInRange(IPAddress ipAddress, IPAddress startAddress, IPAddress endAddress)
        {
            byte[] ipBytes = ipAddress.GetAddressBytes();
            byte[] startBytes = startAddress.GetAddressBytes();
            byte[] endBytes = endAddress.GetAddressBytes();

            bool isInRange = true;
            for (int i = 0; i < ipBytes.Length; i++)
            {
                if (ipBytes[i] < startBytes[i] || ipBytes[i] > endBytes[i])
                {
                    isInRange = false;
                    break;
                }
            }
            return isInRange;
        }
    }
}
