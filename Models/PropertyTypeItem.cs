using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace UmbCodeGen.Models
{
    public class PropertyTypeItem
    {
        [XmlAttribute]
        public int Id { get; set; }
        
        [XmlAttribute]
        public string Alias { get; set; }
       
        [XmlAttribute]
        public int TypeId { get; set; }
       
        public string Name { get; set; }
        
        public string Description { get; set; }

        public DataTypeItem DataType { get; set; }

        public override string ToString()
        {
            return Id.ToString() + " " + Alias;
        }
    }
}