using System.Collections.Generic;
using umbraco.cms.businesslogic.web;
using umbraco.cms.businesslogic.propertytype;
using System.IO;
using umbraco.cms.businesslogic.datatype;
using System.Collections;
using UmbCodeGen.Serialization;
using UmbCodeGen.Models;
using UmbCodeGen.CodeGen;
using System.Web.Script.Serialization;


namespace UmbCodeGen.TypeLib
{
    using umbraco.cms.businesslogic;

    public class DocumentTypeLibrary
    {
        #region Public Methods

        public void Build()
        {
            var typeLib = new DocumentTypeLib();

            BuildDataTypes(typeLib);
            BuildDocumentTypes(typeLib);

            string xml = SerializationHelper.Serialize(typeLib);
            string path = Path.Combine(umbraco.GlobalSettings.FullpathToRoot, "App_Data", "typelib.config");
            File.WriteAllText(path, xml);
        }

        public DocumentTypeLib Load(string typeLibConfigFile)
        {
            DocumentTypeLib documentTypeLib = null;
            if (File.Exists(typeLibConfigFile))
            {
                string xml = File.ReadAllText(typeLibConfigFile);
                if (!string.IsNullOrEmpty(xml))
                    documentTypeLib = SerializationHelper.Deserialize<DocumentTypeLib>(xml);
            }
            return documentTypeLib;
        }

        #endregion
        #region Private Methods

        private void BuildDataTypes(DocumentTypeLib typeLib)
        {
            AddValueTypes(typeLib);
            var dataTypes = DataTypeDefinition.GetAll();
            foreach (var dataType in dataTypes)
            {
                typeLib.DataTypes.Add(this.BuildDataTypeItem(dataType));
            }
        }

        private void AddValueTypes(DocumentTypeLib typeLib)
        {
            typeLib.DataTypes.Add(new DataTypeItem() { Id = -2, ControlTypeName = "umbraco.editorControls.textfield.TextFieldDataType", ModelType = "string", Type = "string" });
            typeLib.DataTypes.Add(new DataTypeItem() { Id = -3, ControlTypeName = "umbraco.editorControls.textfield.TextFieldDataType", ModelType = "int", Type = "string" });
            typeLib.DataTypes.Add(new DataTypeItem() { Id = -4, ControlTypeName = "umbraco.editorControls.textfield.TextFieldDataType", ModelType = "bool", Type = "string" });
            typeLib.DataTypes.Add(new DataTypeItem() { Id = -5, ControlTypeName = "umbraco.editorControls.textfield.TextFieldDataType", ModelType = "double", Type = "string" });
            typeLib.DataTypes.Add(new DataTypeItem() { Id = -6, ControlTypeName = "umbraco.editorControls.textfield.TextFieldDataType", ModelType = "DateTime", Type = "string" });
        }

        private void BuildDocumentTypes(DocumentTypeLib typeLib)
        {
            List<DocumentType> docTypes = DocumentType.GetAllAsList();
            foreach (var documentType in docTypes)
            {
                typeLib.DocumentTypes.Add(this.BuildDocumentTypeItem(documentType));
            }
        }

        private DataTypeItem BuildDataTypeItem(DataTypeDefinition dataTypeDefinition)
        {
            var dataTypeItem = new DataTypeItem();
            dataTypeItem.Id = dataTypeDefinition.DataType.DataTypeDefinitionId;
            dataTypeItem.ControlTypeName = dataTypeDefinition.DataType.GetType().FullName;

            var node = new CMSNode(dataTypeItem.Id);
            dataTypeItem.DataTypeName = node.Text;

            dataTypeItem.PreValueItems = this.BuildPreValues(dataTypeDefinition);
            dataTypeItem.Type = this.DetermineType(dataTypeItem);
            dataTypeItem.ModelType = DetermineModelType(dataTypeItem);
            return dataTypeItem;
        }

        private string DetermineType(DataTypeItem dataType)
        {
            switch (dataType.ControlTypeName)
            {
                case "umbraco.editorControls.colorpicker.ColorPickerDataType": return "string";
                case "umbraco.editorControls.folderbrowser.DataTypeFolderBrowser": return "string";
                case "umbraco.editorControls.imagecropper.DataType": return "string";
                case "umbraco.editorControls.label.DataTypeNoEdit": return "string";
                case "umbraco.editorControls.macrocontainer.DataType": return "string";
                case "umbraco.editorControls.memberpicker.MemberPickerDataType": return "string";
                case "umbraco.editorControls.relatedlinks.RelatedLinksDataType": return "string";
                case "umbraco.editorControls.tinyMCE3.tinyMCE3dataType": return "string";
                case "umbraco.editorControls.simpleEditor.simpleEditorDataType": return "string";
                case "umbraco.editorControls.tags.DataType": return "string";
                case "umbraco.editorControls.textfieldmultiple.textfieldMultipleDataType": return "string";
                case "umbraco.editorControls.textfield.TextFieldDataType": return "string";
                case "umbraco.editorControls.ultimatepicker.ultimatePickerDataType": return "string";
                case "umbraco.editorControls.uploadfield.DataTypeUploadField": return "string";
                case "umbraco.editorControls.XPathDropDownList.XPathDropDownListDataType": return XPathDropDownListDataType(dataType);

                case "umbraco.editorControls.yesno.YesNoDataType": return "bool";

                case "umbraco.editorControls.dropdownlist.DropdownListDataType": return "enum";
                case "umbraco.editorControls.dropdownlist.DropdownListKeysDataType": return "enum";
                case "umbraco.editorControls.checkboxlist.checkboxListDataType": return "List<enum>";
                case "umbraco.editorControls.radiobuttonlist.RadioButtonListDataType": return "enum";
                case "umbraco.editorControls.listbox.ListBoxDataType": return "List<enum>";

                case "umbraco.editorControls.numberfield.IDataTypenteger": return "int";
                case "umbraco.editorControls.pagepicker.PagePickerDataType": return "int";

                case "umbraco.editorControls.datefieldmultiple.DataTypeDatefieldMultiple": return "DateTime";
                case "umbraco.editorControls.datepicker.DateDataType": return "DateTime";

                case "umbraco.editorControls.mediapicker.MemberPickerDataType": return "MediaInfo";
                case "umbraco.editorControls.MultiNodeTreePicker.MNTP_DataType": return "MultiNodePicker";

                case "uComponents.DataTypes.MultiPickerRelations.MultiPickerRelationsDataType": return "string";
                case "uComponents.DataTypes.MultiUrlPicker.MultiUrlPickerDataType": return "MultiUrlPicker";
                case "uComponents.DataTypes.UrlPicker.UrlPickerDataType": return "UrlPicker";
                case "uComponents.DataTypes.Notes.NotesDataType": return "string";
                case "uComponents.DataTypes.DataTypeGrid.DataType": return "DataGrid";
                case "uComponents.DataTypes.MultipleDates.MD_DataType": return "string";
                case "uComponents.DataTypes.SqlCheckBoxList.SqlCheckBoxListDataType": return "SqlCheckBoxList";

                case "Our.Umbraco.GoogleMaps.DataTypes.SingleLocation.SingleLocationDataType": return "string";
            }
            return "string";
        }

        private string DetermineModelType(DataTypeItem dataType)
        {
            switch (dataType.Type)
            {
                case "enum": return Naming.PascalCase(Naming.IdentifierName(dataType.DataTypeName));
                case "List<enum>": return Naming.PascalCase(Naming.IdentifierName(dataType.DataTypeName));
                case "DataGrid": return Naming.PascalCase(Naming.IdentifierName(dataType.DataTypeName));
                case "MultiNodePicker": return "List<int>";
                case "MultiUrlPicker": return "List<HyperLink>";
                case "UrlPicker": return "HyperLink"; 
            }
            switch (dataType.ControlTypeName)
            {
                case "uComponents.DataTypes.MultipleDates.MD_DataType": return "List<DateTime>";
                case "umbraco.editorControls.XPathDropDownList.XPathDropDownListDataType": return this.XPathDropDownListDataType(dataType);
                case "uComponents.DataTypes.SqlCheckBoxList.SqlCheckBoxListDataType": return "List<string>";
            }
            return dataType.Type;
        }

        private string XPathDropDownListDataType(DataTypeItem dataType)
        {
            if (dataType.PreValueItems != null && dataType.PreValueItems.Count > 0)
            {
                var preValue = dataType.PreValueItems[0];
                var json = new JavaScriptSerializer();
                var dictionary = json.Deserialize<Dictionary<string, object>>(preValue.Value);
                if (dictionary.ContainsKey("UseId") && dictionary["UseId"].ToString().ToUpper().Equals("TRUE")) return "int";
            }
            return "string";
        }

        private DocumentTypeItem BuildDocumentTypeItem(DocumentType documentType)
        {
            var documentTypeItem = new DocumentTypeItem();
            documentTypeItem.Alias = documentType.Alias;
            documentTypeItem.Id = documentType.Id;
            documentTypeItem.ParentId = documentType.MasterContentType;
            documentTypeItem.Text = documentType.Text;
            documentTypeItem.Description = documentType.Description;

            foreach (var property in documentType.PropertyTypes)
                documentTypeItem.Properties.Add(this.BuildPropertyTypeItem(property));

            return documentTypeItem;
        }

        private PropertyTypeItem BuildPropertyTypeItem(PropertyType propertyType)
        {
            var propertyTypeItem = new PropertyTypeItem();
            propertyTypeItem.Alias = propertyType.Alias;
            propertyTypeItem.Description = propertyType.Description;
            propertyTypeItem.Id = propertyType.Id;
            propertyTypeItem.Name = propertyType.Name;
            propertyTypeItem.TypeId = GetDataTypeDefinitionId(propertyType);
            return propertyTypeItem;
        }

        private int GetDataTypeDefinitionId(PropertyType propertyType)
        {
            int typeId = DataTypeHelper.GetValueTypeId(propertyType.Alias);
            if (typeId != 0)
                return typeId;

            return propertyType.DataTypeDefinition.Id;
        }

        private List<PreValueItem> BuildPreValues(DataTypeDefinition dataTypeDefinition)
        {
            bool allEmpty = true;
            var prevalues = PreValues.GetPreValues(dataTypeDefinition.DataType.DataTypeDefinitionId);
            if (prevalues != null && prevalues.Count > 0)
            {
                var preValueItems = new List<PreValueItem>();
                foreach (DictionaryEntry item in prevalues)
                {
                    var preValue = item.Value as PreValue;
                    if (preValue != null)
                    {
                        var preValueItem = new PreValueItem()
                            {
                                Id = preValue.Id,
                                SortOrder = preValue.SortOrder,
                                Value = preValue.Value
                            };
                        preValueItems.Add(preValueItem);
                        if (!string.IsNullOrEmpty(preValueItem.Value)) allEmpty = false;
                    }

                }
                if (allEmpty) return null;

                return preValueItems;
            }
            return null;
        }

        #endregion
    }

}