using System;
using System.Runtime.Serialization;

namespace EmptyService.CommonEntities.Exceptions
{
    // ReSharper disable once AllowPublicClass
    public class ImpossibleSituationException : BusinessException
    {
        public ImpossibleSituationException() { }

        public ImpossibleSituationException(string message)
            : base(message) { }

        public ImpossibleSituationException(string message, Exception innerException)
            : base(message, innerException) { }

        protected ImpossibleSituationException(SerializationInfo info, StreamingContext context)
            : base(info, context) { }
    }
}