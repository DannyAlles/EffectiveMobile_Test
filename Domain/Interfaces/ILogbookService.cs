using Domain.Models;
using Domain.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface ILogbookService
    {
        public Task<IEnumerable<LogbookByIpViewModel>> GetLogsByIpAsync(string startIp, string endIp, DateTimeOffset startDate, DateTimeOffset endDate);
        public Task<MemoryStream> GetFileLogsByIpAsync(string startIp, string endIp, DateTimeOffset startDate, DateTimeOffset endDate);
    }
}
