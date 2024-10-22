using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.ViewModels
{
    public class LogbookByIpViewModel
    {
        public string IpAddress { get; set; }
        public DateTimeOffset AccessAt { get; set; }
        public int Count { get; set; }
    }
}
