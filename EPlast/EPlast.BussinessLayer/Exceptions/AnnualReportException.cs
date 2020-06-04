using System;
using System.Runtime.Serialization;

namespace EPlast.BussinessLayer.Exceptions
{
    public class AnnualReportException : Exception, ISerializable
    {
        public AnnualReportException(string message) : base(message)
        {

        }

        protected AnnualReportException(SerializationInfo info, StreamingContext context) : base(info, context)
        {

        }
    }
}