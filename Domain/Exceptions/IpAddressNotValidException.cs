using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Exceptions
{
    [Serializable]
    public class IpAddressNotValidException : Exception
    {
        public IpAddressNotValidException()
        {
        }

        public IpAddressNotValidException(string? message) : base(message)
        {
        }

        public IpAddressNotValidException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected IpAddressNotValidException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
