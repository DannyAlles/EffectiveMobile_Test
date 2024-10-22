using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Exceptions
{
    [Serializable]
    public class DistrictNotFoundException : Exception
    {
        public DistrictNotFoundException()
        {
        }

        public DistrictNotFoundException(string? message) : base(message)
        {
        }

        public DistrictNotFoundException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected DistrictNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
