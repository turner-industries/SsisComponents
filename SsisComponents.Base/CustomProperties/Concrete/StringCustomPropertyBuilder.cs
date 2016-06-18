using Microsoft.SqlServer.Dts.Pipeline.Wrapper;
using SsisComponents.Base.CustomProperties.Abstract;

namespace SsisComponents.Base.CustomProperties.Concrete
{
    public class StringCustomPropertyBuilder : BaseCustomPropertyBuilder
    {
        private readonly string _defaultValue;

        public StringCustomPropertyBuilder(string defaultValue) 
        {
            Value = defaultValue;
        }
    }
}