using System.Linq;
using System.IO;
using System.Collections.Generic;
using UmbCodeGen.TypeLib;
using UmbCodeGen.Models;
using System.Web.Script.Serialization;
using System.Text.RegularExpressions;

namespace UmbCodeGen.CodeGen
{
    using System;

    public class CodeGenTools
    {
        #region Constructors
        public CodeGenTools(object textTransformation)
        {
            if (textTransformation != null)
            {
                _textTransformation = DynamicTextTransformation.Create(textTransformation);
            }
        }

        #endregion
        #region Public Methods

        public bool LoadMetaData(string configFile)
        {
            try
            {
                configFile = _textTransformation.Host.ResolvePath(configFile);
                if (!string.IsNullOrEmpty(configFile) && File.Exists(configFile))
                {
                    Directory.SetCurrentDirectory(Path.GetDirectoryName(configFile));
                    var documentTypeLibrary = new DocumentTypeLibrary();
                    TypeLib = documentTypeLibrary.Load(configFile);
                    this.SetPropertyTypeDataType(TypeLib);
                    DataGridTypeItems = this.BuildDataGridTypeItemList(TypeLib);
                }
            }
            catch (Exception)
            {
                TypeLib = null;
            }
            return TypeLib != null;
        }

        public string ExcludePropertyRegEx { get; set; }
        public DocumentTypeLib TypeLib { get; set; }

        public IEnumerable<DataTypeItem> GetEnumDataTypes()
        {
            foreach (var dataType in TypeLib.DataTypes.Where(dt => dt.Type.Equals("enum") || dt.Type.Equals("List<enum>")))
            {
                yield return dataType;

            }
            yield break;
        }

        public IEnumerable<DataGridTypeItem> GetDataGridTypeItems()
        {
            foreach (var gridType in DataGridTypeItems)
            {
                yield return gridType;

            }
            yield break;
        }

        public IEnumerable<DocumentTypeItem> GetDocumentTypes()
        {
            foreach (var docType in TypeLib.DocumentTypes)
            {
                DocumentTypeItem = docType;
                yield return docType;

            }
            yield break;
        }

        public bool IncludeProperty(PropertyTypeItem propertyType, string className)
        {
            if (string.IsNullOrEmpty(ExcludePropertyRegEx)) return true;

            string identifierName = this.PropertyName(propertyType, className);
            Regex regex = new Regex(ExcludePropertyRegEx, RegexOptions.CultureInvariant | RegexOptions.ExplicitCapture | RegexOptions.IgnoreCase);
            MatchCollection matchCollection = regex.Matches(identifierName);
            return matchCollection.Count == 0;
        }

        public string NameSpace()
        {
            string suggestion = _textTransformation.Host.ResolveParameterValue("directiveId", "namespaceDirectiveProcessor", "namespaceHint");
            if (string.IsNullOrEmpty(suggestion))
            {
                return null;
            }
            return suggestion.Replace(".Generated", string.Empty);
        }

        public string ClassName(string alias)
        {
            return Naming.PascalCase(Naming.IdentifierName(alias));
        }

        public string PropertyName(PropertyTypeItem propertyType, string className)
        {
            return Naming.PropertyName(propertyType.Alias, className);
        }

        public string PascalCase(string identifier)
        {
            return Naming.PascalCase(identifier);
        }

        public string IdentifierName(string alias)
        {
            return Naming.IdentifierName(alias);
        }


        #endregion
        #region Private Methods

        private List<DataGridTypeItem> DataGridTypeItems { get; set; }
        private DocumentTypeItem DocumentTypeItem { get; set; }

        private List<DataGridTypeItem> BuildDataGridTypeItemList(DocumentTypeLib typeLib)
        {
            var dataGridTypeItems = new List<DataGridTypeItem>();
            foreach (var docType in TypeLib.DocumentTypes)
            {
                string className = this.ClassName(docType.Alias);

                foreach (var propType in docType.Properties)
                {
                    if (IncludeProperty(propType, className))
                    {
                        if (propType.DataType.Type.Equals("DataGrid"))
                        {
                            if (dataGridTypeItems.Any(dg => dg.TypeId == propType.TypeId) == false)
                            {
                                var dataGridTypeItem = CreateDataGridTypeItem(propType, className);
                                if (dataGridTypeItem != null)
                                    dataGridTypeItems.Add(dataGridTypeItem);
                            }
                        }
                    }
                }
            }
            return dataGridTypeItems;
        }

        private DataGridTypeItem CreateDataGridTypeItem(PropertyTypeItem propType, string className)
        {
            if (propType.DataType.PreValueItems.Count > 1)
            {
                var dataGridTypeItem = new DataGridTypeItem();
                dataGridTypeItem.Id = propType.Id;
                dataGridTypeItem.TypeId = propType.TypeId;
                dataGridTypeItem.ClassName = className;
                dataGridTypeItem.IdentifierName = Naming.PascalCase(Naming.IdentifierName(propType.DataType.DataTypeName));

                foreach (var preValue in propType.DataType.PreValueItems.Skip(1))
                {
                    var column = new DataGridTypeItem.Column();
                    var json = new JavaScriptSerializer();
                    var dictionary = json.Deserialize<Dictionary<string, object>>(preValue.Value);
                    if (dictionary.ContainsKey("Alias"))
                    {
                        column.Alias = dictionary["Alias"] as string;
                        column.IdentifierName = Naming.PropertyName(column.Alias, className);
                    }
                    if (dictionary.ContainsKey("DataTypeId"))
                    {
                        int typeId = DataTypeHelper.GetValueTypeId(column.Alias);
                        if (typeId == 0)
                        {
                            column.DataType = TypeLib.DataTypes.FirstOrDefault(dt => dt.Id == (int)dictionary["DataTypeId"]);
                        }
                        else
                        {
                            column.DataType = TypeLib.DataTypes.FirstOrDefault(dt => dt.Id == typeId);
                            column.DataType.Type = column.DataType.ModelType;
                        }
                    }
                    dataGridTypeItem.Columns.Add(column);
                }
                return dataGridTypeItem;
            }
            return null;
        }

        private void SetPropertyTypeDataType(DocumentTypeLib typeLib)
        {
            foreach (var docType in typeLib.DocumentTypes)
            {
                foreach (var propType in docType.Properties)
                {
                    propType.DataType = typeLib.DataTypes.FirstOrDefault(dt => dt.Id == propType.TypeId);
                }
            }
        }

        #endregion


        #region Private Members
        private readonly DynamicTextTransformation _textTransformation;
        #endregion
    }
}
