using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UmbCodeGen.Models
{
    public class DocumentTypeLib
    {
        public DocumentTypeLib()
        {
            DocumentTypes = new List<DocumentTypeItem>();
            DataTypes = new List<DataTypeItem>();
        }

        public List<DataTypeItem> DataTypes { get; set; } 
        public List<DocumentTypeItem> DocumentTypes { get; set; } 
    }
}