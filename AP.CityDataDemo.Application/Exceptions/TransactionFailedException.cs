using System;
using System.Runtime.Serialization;

namespace AP.CityDataDemo.Application.Exceptions
{
    [Serializable]
    public class TransactionFailedException : Exception
    {
        public TransactionFailedException()
        {
        }

        public TransactionFailedException(string message)
            : base(message)
        {
        }
    }
}