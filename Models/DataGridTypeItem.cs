using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UmbCodeGen.Models
{
    public class DataGridTypeItem
    {
        public DataGridTypeItem()
        {
            Columns = new List<Column>();
        }

        public class Column
        {
            public string IdentifierName { get; set; }
            public string Alias { get; set; }
            public DataTypeItem DataType { get; set; }
        }

        public int Id { get; set; }
        public int TypeId { get; set; }
        public string ClassName { get; set; }
        public string IdentifierName { get; set; }
        public List<Column> Columns;
    }
}
