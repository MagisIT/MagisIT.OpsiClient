using System;

namespace OpsiClientSharp.Exceptions
{
    public class OpsiClientRequestException : Exception
    {
        public OpsiClientRequestException() { }

        public OpsiClientRequestException(string message) : base(message) { }
    }
}
