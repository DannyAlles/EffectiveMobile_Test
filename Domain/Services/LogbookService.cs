using Domain.Interfaces;
using Domain.Models;
using Domain.Exceptions;
using Domain.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Services
{
    public class LogbookService : ILogbookService
    {
        private readonly ILogbookRepository _logbookRepository;

        public LogbookService(ILogbookRepository logbookRepository)
        {
            _logbookRepository = logbookRepository ?? throw new ArgumentNullException(nameof(logbookRepository));
        }

        public async Task<IEnumerable<LogbookByIpViewModel>> GetLogsByIpAsync(string startIp, string endIp, DateTimeOffset startDate, DateTimeOffset endDate)
        {
            if (!IsValidIpAddress(startIp) || !IsValidIpAddress(endIp)) throw new IpAddressNotValidException();

            IPAddress startAddress = IPAddress.Parse(startIp);
            IPAddress endAddress = IPAddress.Parse(endIp);

            var logbooks = await _logbookRepository.GetLogbooksByIpAsync(startAddress, endAddress, startDate.DateTime, endDate.DateTime).ConfigureAwait(false);

            return logbooks.Select(x => new LogbookByIpViewModel()
            {
                AccessAt = x.AccessAt,
                IpAddress = x.IpAddress,
                Count = logbooks.Count(c => c.IpAddress == x.IpAddress)
            }).DistinctBy(x => x.IpAddress);
        }

        public async Task<MemoryStream> GetFileLogsByIpAsync(string startIp, string endIp, DateTimeOffset startDate, DateTimeOffset endDate)
        {
            if (!IsValidIpAddress(startIp) || !IsValidIpAddress(endIp)) throw new IpAddressNotValidException();

            IPAddress startAddress = IPAddress.Parse(startIp);
            IPAddress endAddress = IPAddress.Parse(endIp);

            var logbooks = await _logbookRepository.GetLogbooksByIpAsync(startAddress, endAddress, startDate.DateTime, endDate.DateTime).ConfigureAwait(false);

            return WriteOrdersToMemoryStream(logbooks.Select(x => new LogbookByIpViewModel()
            {
                AccessAt = x.AccessAt,
                IpAddress = x.IpAddress,
                Count = logbooks.Count(c => c.IpAddress == x.IpAddress)
            }).DistinctBy(x => x.IpAddress));
        }

        private static bool IsValidIpAddress(string ipAddress)
        {
            if (!IPAddress.TryParse(ipAddress, out _)) return false;

            var parts = ipAddress.Split('.');
            if (parts.Length != 4) return false;

            foreach (var part in parts)
                if (!int.TryParse(part, out _)) return false;

            foreach (var part in parts)
                if (int.Parse(part) < 0 || int.Parse(part) > 255) return false;

            return true;
        }

        private static MemoryStream WriteOrdersToMemoryStream(IEnumerable<LogbookByIpViewModel> logbooks)
        {
            var memoryStream = new MemoryStream();
            using (var writer = new StreamWriter(memoryStream, Encoding.UTF8, leaveOpen: true))
            {
                foreach (var logbook in logbooks)
                {
                    writer.WriteLine($"{logbook.IpAddress}: {logbook.AccessAt.ToString("yyyy-MM-dd HH:mm:ss")}\n" +
                        $"Count: {logbook.Count}\n");
                }
                writer.Flush();
            }
            return memoryStream;
        }
    }
}
