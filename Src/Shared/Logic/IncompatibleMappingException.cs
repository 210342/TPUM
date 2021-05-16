using System;
using System.Runtime.Serialization;

namespace TPUM.Shared.Logic
{
    public class IncompatibleMappingException : Exception
    {
        public IncompatibleMappingException()
        {
        }

        public IncompatibleMappingException(string message) : base(message)
        {
        }

        public IncompatibleMappingException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected IncompatibleMappingException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
