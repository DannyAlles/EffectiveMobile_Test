using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class DeliveryConfigurationOptions
    {
        public ConnectionStringsConfigurationOptions ConnectionStrings { get; set; }
        public OrderOptions OrderOptions { get; set; }
    }

    public class ConnectionStringsConfigurationOptions
    {
        public string Postgres { get; set; }
    }

    public class OrderOptions
    {
        public int DecimalPlacesNum { get; set; }
        public int MinutesAfterFirst { get; set; }
    }
}
