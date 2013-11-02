using System;

namespace UmbCodeGen.TypeLib
{
    public class DataTypeException : Exception
    {
        public DataTypeException() : base()
        {
            
        }

        public DataTypeException(string message) : base(message)
        {

        }

        public DataTypeException(string message, Exception innerException) : base(message, innerException)
        {
            
        }
    }
}
