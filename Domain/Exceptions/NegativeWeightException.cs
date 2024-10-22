using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Exceptions
{
    [Serializable]
    public class NegativeWeightException : Exception
    {
        public NegativeWeightException()
        {
        }

        public NegativeWeightException(string? message) : base(message)
        {
        }

        public NegativeWeightException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected NegativeWeightException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
