using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UmbCodeGen.TypeLib
{
    public class DocumentTypeException : Exception
    {
        public DocumentTypeException() : base()
        {

        }

        public DocumentTypeException(string message) : base(message)
        {

        }

        public DocumentTypeException(string message, Exception innerException)
            : base(message, innerException)
        {

        }
    }
}
