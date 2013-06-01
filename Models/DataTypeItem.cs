using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace UmbCodeGen.Models
{
    public class DataTypeItem
    {
        [XmlAttribute]
        public int Id { get; set; }                     // [cmsDataType].[nodeId]
        [XmlAttribute]
        public string ControlTypeName { get; set; }     // umbraco control type full name
        [XmlAttribute]
        public string Type { get; set; }                // Abstract: type of item
        [XmlAttribute]
        public string ModelType { get; set; }           // .NET type / class / enum
        [XmlAttribute]
        public string DataTypeName { get; set; }        // CMSNode.Name

        public List<PreValueItem> PreValueItems { get; set; }

    }
}
