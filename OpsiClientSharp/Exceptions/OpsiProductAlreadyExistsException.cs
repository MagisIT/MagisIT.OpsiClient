using System;

namespace OpsiClientSharp.Exceptions
{
    public class OpsiProductAlreadyExistsException : Exception
    {
        public OpsiProductAlreadyExistsException() { }

        public OpsiProductAlreadyExistsException(string message) : base(message) { }
    }
}
