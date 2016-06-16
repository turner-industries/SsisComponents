using Microsoft.SqlServer.Dts.Pipeline.Wrapper;

namespace SsisComponents.Base.CustomProperties.Abstract
{
    public interface ICustomPropertyBuilder
    {
        IDTSCustomProperty100 Build(IDTSCustomPropertyCollection100 collection);
    }
}