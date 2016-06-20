using Microsoft.SqlServer.Dts.Pipeline.Wrapper;
using SsisComponents.Base.CustomProperties.Abstract;

namespace SsisComponents.Base.CustomProperties.Concrete
{
    public class BooleanCustomPropertyBuilder : BaseCustomPropertyBuilder
    {
        public BooleanCustomPropertyBuilder(
            bool initialState)
        {
            Value = initialState;
        }
    }
}