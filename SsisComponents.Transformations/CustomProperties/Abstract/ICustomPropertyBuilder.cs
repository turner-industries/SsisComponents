using Microsoft.SqlServer.Dts.Pipeline.Wrapper;

namespace SsisComponents.Transformations.CustomProperties.Abstract
{
    public interface ICustomPropertyBuilder
    {
        IDTSCustomProperty100 Build(IDTSCustomPropertyCollection100 collection);
    }
}