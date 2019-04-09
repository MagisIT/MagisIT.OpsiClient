using System;
using System.Runtime.Serialization;

namespace MagisIT.OpsiClient.Exceptions
{
    public class OpsiProductAlreadyExistsException : Exception
    {
        public OpsiProductAlreadyExistsException() { }

        public OpsiProductAlreadyExistsException(string message) : base(message) { }

        protected OpsiProductAlreadyExistsException(SerializationInfo info, StreamingContext context) : base(info, context) { }

        public OpsiProductAlreadyExistsException(string message, Exception innerException) : base(message, innerException) { }
    }
}
