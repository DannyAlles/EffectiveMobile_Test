using Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Data.Interfaces
{
    public interface ILogbookRepository
    {
        public Task CreateLogbookAsync(Logbook logbook);
        public Task<IEnumerable<Logbook>> GetLogbooksByIpAsync(IPAddress startAddress, IPAddress endAddress, DateTime startDate, DateTime endDate);
    }
}
