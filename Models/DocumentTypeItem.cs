using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace UmbCodeGen.Models
{
    public class DocumentTypeItem
    {
        public DocumentTypeItem()
        {
            Properties = new List<PropertyTypeItem>();
        }

        [XmlAttribute]
        public int Id { get; set; }

        [XmlAttribute]
        public int ParentId { get; set; }

        [XmlAttribute]
        public string Alias { get; set; }
        public string Text { get; set; }
        public string Description { get; set; }

        public List<PropertyTypeItem> Properties { get; set; }

        public override string ToString()
        {
            return Id.ToString() + " " + Alias;
        }
    }

}