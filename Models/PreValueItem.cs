using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace UmbCodeGen.Models
{
    //[Serializable, XmlRoot("DataTypeInfo")]
    public class PreValueItem
    {
        [XmlAttribute]
        public int Id { get; set; }
        [XmlAttribute]
        public int SortOrder { get; set; }

        public string Value { get; set; }

        public override string ToString()
        {
            return Value;
        }
    }
}