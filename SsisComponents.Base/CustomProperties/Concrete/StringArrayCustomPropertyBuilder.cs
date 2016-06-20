using Microsoft.SqlServer.Dts.Pipeline.Wrapper;
using SsisComponents.Base.CustomProperties.Abstract;

namespace SsisComponents.Base.CustomProperties.Concrete
{
    public class StringArrayCustomPropertyBuilder : BaseCustomPropertyBuilder
    {
        public StringArrayCustomPropertyBuilder()
        {
            Value = new string[0];
        }
    }
}
