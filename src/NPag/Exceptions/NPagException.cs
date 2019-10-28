using System;
using System.Runtime.Serialization;

namespace NPag.Exceptions
{
    public class NPagException : Exception
    {
        public NPagException()
        {
        }

        protected NPagException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        public NPagException(string message) : base(message)
        {
        }

        public NPagException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}