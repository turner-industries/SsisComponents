using System.Collections.Generic;
using System.Linq;
using Microsoft.SqlServer.Dts.Pipeline.Wrapper;
using Microsoft.SqlServer.Dts.Runtime.Wrapper;
using SsisComponents.Base.Adapters.Abstract;
using SsisComponents.Base.CustomProperties.Abstract;
using SsisComponents.Base.CustomProperties.Factory;

namespace SsisComponents.Base.Adapters.Concrete
{
    public class ComponentMetadataAdapter : IComponentMetadataAdapter
    {
        
        private IDTSComponentMetaData100 _componentMetaData;
        private CustomPropertyFactory _customPropertyFactory;

        public void Initialize(IDTSComponentMetaData100 componentMetaData)
        {
            _componentMetaData = componentMetaData;
            _customPropertyFactory = new CustomPropertyFactory(componentMetaData.CustomPropertyCollection);
        }

        public TValue GetValueOfCustomProperty<TValue>(string propertyName)
        {
            return (TValue) _componentMetaData
                .CustomPropertyCollection
                .Cast<IDTSCustomProperty100>()
                .Single(p => p.Name.Equals(propertyName))
                .Value;
        }

        public IEnumerable<IDTSInputColumn100> GetInputColumns()
        {
            return _componentMetaData
                .InputCollection
                .Cast<IDTSInput100>()
                .SelectMany(i => i.InputColumnCollection.Cast<IDTSInputColumn100>());
        }

        public IDTSOutputColumn100 CreateOutputColumnFromInputColumn(
            string newColumnName,
            IDTSInputColumn100 inputColumn)
        {
            var outPutColumn = _componentMetaData
                .OutputCollection[0]
                .OutputColumnCollection
                .New();

            outPutColumn.Name = newColumnName;
            outPutColumn.SetDataTypeProperties(
                inputColumn.DataType,
                inputColumn.Length,
                inputColumn.Precision,
                inputColumn.Scale,
                inputColumn.CodePage);

            return outPutColumn;
        }

        public IEnumerable<IDTSInputColumn100> GetInputColumnsByInputId(int inputID)
        {
            return _componentMetaData
                .InputCollection
                .GetObjectByID(inputID)
                .InputColumnCollection
                .Cast<IDTSInputColumn100>();
        }

        public int GetInputColumnIndex(IDTSInputColumn100 inputColumn)
        {
            var input = _componentMetaData
                .InputCollection
                .Cast<IDTSInput100>()
                .Single(i => i.InputColumnCollection.Cast<IDTSInputColumn100>()
                    .Any(c => c.Name == inputColumn.Name));

            return input
                .InputColumnCollection
                .FindObjectIndexByID(inputColumn.ID);
        }

        public int GetOutputColumnIndex(IDTSOutputColumn100 outputColumn)
        {
            var output = _componentMetaData
                .OutputCollection
                .Cast<IDTSOutput100>()
                .Single(o => o.OutputColumnCollection.Cast<IDTSOutputColumn100>()
                    .Any(c => c.Name == outputColumn.Name));

            return output
                .OutputColumnCollection
                .FindObjectIndexByID(outputColumn.ID);
        }

        public void RemoveOutputColumnByIndex(int index)
        {
            _componentMetaData
                .OutputCollection[0]
                .OutputColumnCollection
                .RemoveObjectByIndex(index);
        }

        public void CheckAllInputColumns(int inputID, params DataType[] restrictToDataTypes)
        {
            var input = GetInputByInputID(inputID)
                .GetVirtualInput();

            var columns = restrictToDataTypes != null
                ? input.VirtualInputColumnCollection
                    .Cast<IDTSVirtualInputColumn100>()
                    .Where(v => restrictToDataTypes.Any(d => v.DataType == d))
                : input.VirtualInputColumnCollection
                    .Cast<IDTSVirtualInputColumn100>();

            foreach (IDTSVirtualInputColumn100 virtualInputColumn in columns)
            {
                
                input.SetUsageType(virtualInputColumn.LineageID, DTSUsageType.UT_READONLY);
            }
        }

        public IEnumerable<IDTSOutputColumn100> GetOutputColumns()
        {
            return _componentMetaData
                .OutputCollection
                .Cast<IDTSOutput100>()
                .SelectMany(i => i.OutputColumnCollection.Cast<IDTSOutputColumn100>());
        }

        public void AddNewCustomPropertyToOutputColumn(
            IDTSOutputColumn100 dtsOutputColumn100, 
            string propertyName, 
            object propertyValue)
        {
            var newProperty = dtsOutputColumn100.CustomPropertyCollection.New();
            newProperty.Name = propertyName;
            newProperty.Value = propertyValue;
        }

        public TValue GetCustomPropertyFromOutputColumn<TValue>(IDTSOutputColumn100 outputColumn, string propertyName)
        {
            return (TValue) outputColumn
                .CustomPropertyCollection
                .Cast<IDTSCustomProperty100>()
                .Single(p => p.Name.Equals(propertyName))
                .Value;
        }

        public IEnumerable<IDTSOutputColumn100> GetOutputColumns(int outputID)
        {
            return _componentMetaData
                .OutputCollection
                .GetObjectByID(outputID)
                .OutputColumnCollection
                .Cast<IDTSOutputColumn100>();
        }

        public void AddNewCustomPropertyToOutput(int outputID, string propertyName, object propertyValue)
        {
            var output = _componentMetaData
                .OutputCollection
                .GetObjectByID(outputID);

            var newProperty = output.CustomPropertyCollection.New();
            newProperty.Name = propertyName;
            newProperty.Value = propertyValue;
        }

        public IDTSInputColumn100 GetInputColumnByName(string columnName)
        {
            return GetInputColumns()
                .SingleOrDefault(c => c.Name.Equals(columnName));
        }

        private IDTSInput100 GetInputByInputID(int inputID)
        {
            return _componentMetaData
                .InputCollection
                .GetObjectByID(inputID);
        }

        public void AddNewCustomDesignTimeProperty(ICustomPropertyBuilder customPropertyBuilder)
        {
            _customPropertyFactory.Create(customPropertyBuilder);
        }
    }
}
