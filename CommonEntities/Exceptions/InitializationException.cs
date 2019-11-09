using System;
using System.Runtime.Serialization;

namespace EmptyService.CommonEntities.Exceptions
{
    // ReSharper disable once AllowPublicClass
    public class InitializationException : BusinessException
    {
        public InitializationException() { }

        public InitializationException(string message)
            : base(message) { }

        public InitializationException(string message, Exception innerException)
            : base(message, innerException) { }

        protected InitializationException(SerializationInfo info, StreamingContext context)
            : base(info, context) { }
    }
}