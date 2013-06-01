using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UmbCodeGen.CodeGen
{
    public class DataTypeHelper
    {
        public static int GetValueTypeId(string alias)
        {
            if (alias.StartsWith("string_")) return -2;
            if (alias.StartsWith("int_")) return -3;
            if (alias.StartsWith("integer_")) return -3;
            if (alias.StartsWith("bool_")) return -4;
            if (alias.StartsWith("double_")) return -5;
            if (alias.StartsWith("datetime_")) return -6;

            if (alias.EndsWith("_string")) return -2;
            if (alias.EndsWith("_int")) return -3;
            if (alias.EndsWith("_integer")) return -3;
            if (alias.EndsWith("_bool")) return -4;
            if (alias.EndsWith("_double")) return -5;
            if (alias.EndsWith("_datetime")) return -6;

            return 0;
        }
    }
}
