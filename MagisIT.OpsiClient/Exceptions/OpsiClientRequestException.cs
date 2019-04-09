using System;
using System.Runtime.Serialization;

namespace MagisIT.OpsiClient.Exceptions
{
    public class OpsiClientRequestException : Exception
    {
        public OpsiClientRequestException() { }

        public OpsiClientRequestException(string message) : base(message) { }

        protected OpsiClientRequestException(SerializationInfo info, StreamingContext context) : base(info, context) { }

        public OpsiClientRequestException(string message, Exception innerException) : base(message, innerException) { }
    }
}
