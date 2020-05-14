using System;
using System.Runtime.Serialization;

namespace GeoLearn.Models
{
    public class MineralException : Exception
    {
        const string msgStart = "Mineral exception: ";
        public MineralException() : base(msgStart + "unknown exception")
        {
        }

        public MineralException(string message) : base(msgStart + message)
        {
        }

        public MineralException(string message, Exception innerException) : base(msgStart + message, innerException)
        {
        }

        protected MineralException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}