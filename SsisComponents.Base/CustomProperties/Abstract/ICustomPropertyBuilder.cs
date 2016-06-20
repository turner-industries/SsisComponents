using Microsoft.SqlServer.Dts.Pipeline.Wrapper;

namespace SsisComponents.Base.CustomProperties.Abstract
{
    public interface ICustomPropertyBuilder
    {
        string PropertyName { get; set; }
        string PropertyDescription { get; set; }
        DTSPersistState PersistState { get; set; }
        object Value { get; set; }

        IDTSCustomProperty100 Build(IDTSCustomPropertyCollection100 collection);
    }
}