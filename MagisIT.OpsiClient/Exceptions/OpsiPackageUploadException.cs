using System;
using System.Runtime.Serialization;

namespace MagisIT.OpsiClient.Exceptions
{
    public class OpsiPackageUploadException : Exception
    {
        public OpsiPackageUploadException() { }

        public OpsiPackageUploadException(string message) : base(message) { }

        protected OpsiPackageUploadException(SerializationInfo info, StreamingContext context) : base(info, context) { }

        public OpsiPackageUploadException(string message, Exception innerException) : base(message, innerException) { }
    }
}
