using System;
using System.Runtime.Serialization;

namespace EPlast.BussinessLayer.Exceptions
{
    [Serializable]
    public class AnnualReportException : Exception
    {
        public AnnualReportException(string message) : base(message)
        {

        }

        protected AnnualReportException(SerializationInfo info, StreamingContext context) : base(info, context)
        {

        }
    }
}