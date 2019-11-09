using System;
using System.Runtime.Serialization;

namespace EmptyService.CommonEntities.Exceptions
{
    // ReSharper disable once AllowPublicClass
    public class BusinessException : Exception
    {
        public BusinessException() { }

        public BusinessException(string message)
            : base(message) { }

        public BusinessException(string message, Exception innerException)
            : base(message, innerException) { }

        protected BusinessException(SerializationInfo info, StreamingContext context)
            : base(info, context) { }
    }
}