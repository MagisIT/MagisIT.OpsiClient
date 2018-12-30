using System;

namespace OpsiClientSharp.Exceptions
{
    public class OpsiPackageUploadException : Exception
    {
        public OpsiPackageUploadException() { }

        public OpsiPackageUploadException(string message) : base(message) { }
    }
}
