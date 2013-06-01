using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;
using System.Text.RegularExpressions;

namespace UmbCodeGen.CodeGen
{
    public static class Naming
    {
        public static string PropertyName(string alias, string className)
        {
            string identiefier = StripTypes(alias);
            identiefier = IdentifierName(identiefier);
            identiefier = PascalCase(identiefier);
            if (identiefier.StartsWith(className)) identiefier = identiefier.Substring(className.Length);

            return identiefier;
        }

        public static string IdentifierName(string alias)
        {
            bool toUpper = false;
            string name = null;
            foreach (char c in alias.ToCharArray())
            {
                if (!IsAllowedIdentifierChar(c))
                {
                    toUpper = true;
                }
                else
                {
                    if (toUpper)
                    {
                        name += c.ToString().ToUpper();
                        toUpper = false;
                    }
                    else
                    {
                        name += c;
                    }
                }
            }

            //If a Indentifier starts with a non allowed characted (for example the enum prevalue "3D"), prefix the name with an underscore.
            if (StarsWithIllegalChar(name))
                name = "_" + name;
            return name;
        }

        public static string PascalCase(string identifier)
        {
            if (String.IsNullOrEmpty(identifier))
            {
                return identifier;
            }

            if (identifier.Length == 1)
            {
                return identifier[0].ToString(CultureInfo.InvariantCulture).ToUpperInvariant();
            }

            return identifier[0].ToString(CultureInfo.InvariantCulture).ToUpperInvariant() + identifier.Substring(1);
        }

        /// <summary>
        /// Strip a type idetification from the property name. 
        /// </summary>
        /// <param name="propertyName">The property name in PascalCase</param>
        /// <returns>the stripped property name</returns>
        private static string StripTypes(string propertyName)
        {
            propertyName = StripType(propertyName, "string");
            propertyName = StripType(propertyName, "int");
            propertyName = StripType(propertyName, "integer");
            propertyName = StripType(propertyName, "bool");
            propertyName = StripType(propertyName, "double");
            propertyName = StripType(propertyName, "datetime");

            return propertyName;
        }

        private static string StripType(string propertyName, string type)
        {
            if (propertyName.StartsWith(type + "_")) return propertyName.Substring(type.Length);
            if (propertyName.StartsWith(type + "-")) return propertyName.Substring(type.Length);
            if (propertyName.EndsWith("_" + type)) return propertyName.Substring(0, propertyName.Length - type.Length);
            if (propertyName.EndsWith("-" + type)) return propertyName.Substring(0, propertyName.Length - type.Length);
            return propertyName;
        }

        private static bool StarsWithIllegalChar(string identifierName)
        {
            return regexStartIllegalChar.IsMatch(identifierName);
        }

        private static bool IsAllowedIdentifierChar(char character)
        {
            return regexIdentifierChar.IsMatch(character.ToString());
        }

        private static Regex regexStartIllegalChar = new Regex("^[^a-z|A-Z|_]", RegexOptions.Compiled);
        private static Regex regexIdentifierChar = new Regex("^[a-z|A-Z|0-9]", RegexOptions.Compiled);

    }
}
