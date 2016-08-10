
namespace Sleemon.Common
{
    using System;

    using System.Runtime.Serialization;

    public class InvalidArgumentException : Exception
    {
        public InvalidArgumentException() : base() { }

        public InvalidArgumentException(string message) : base(message) { }

        public InvalidArgumentException(string message, Exception innerException):base(message, innerException) { }

        protected InvalidArgumentException(SerializationInfo info, StreamingContext context):base(info, context) { }
    }
}
