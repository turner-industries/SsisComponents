using System.Collections.Generic;
using Microsoft.SqlServer.Dts.Pipeline.Wrapper;
using Microsoft.SqlServer.Dts.Runtime.Wrapper;
using SsisComponents.Base.CustomProperties.Abstract;

namespace SsisComponents.Base.Adapters.Abstract
{
    public interface IComponentMetadataAdapter
    {
        void AddNewCustomDesignTimeProperty(ICustomPropertyBuilder customPropertyBuilder);
        void Initialize(IDTSComponentMetaData100 componentMetaData);
        TValue GetValueOfCustomProperty<TValue>(string columns);
        IEnumerable<IDTSInputColumn100> GetInputColumns();
        IDTSOutputColumn100 CreateOutputColumnFromInputColumn(string newColumnName, IDTSInputColumn100 inputColumn);
        IEnumerable<IDTSInputColumn100> GetInputColumnsByInputId(int inputID);
        int GetInputColumnIndex(IDTSInputColumn100 inputColumn);
        int GetOutputColumnIndex(IDTSOutputColumn100 destinationOuputColumn);
        void CheckAllInputColumns(int inputID, params DataType[] restrictToDataTypes);
        IEnumerable<IDTSOutputColumn100> GetOutputColumns();
        void AddNewCustomPropertyToOutputColumn(IDTSOutputColumn100 dtsOutputColumn100, string propertyName, object propertyValue);
        TValue GetCustomPropertyFromOutputColumn<TValue>(IDTSOutputColumn100 outputColumn, string propertyName);
        IDTSInputColumn100 GetInputColumnByName(string columnName);
    }
}
