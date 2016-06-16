using Microsoft.SqlServer.Dts.Pipeline.Wrapper;
using SsisComponents.Base.CustomProperties.Abstract;

namespace SsisComponents.Base.CustomProperties.Concrete
{
    public class StringCustomPropertyBuilder : BaseCustomPropertyBuilder
    {
        private readonly string _defaultValue;

        public StringCustomPropertyBuilder(
            string propertyName, 
            string propertyDescription, 
            DTSPersistState persistState,
            string defaultValue) 
                : base(propertyName, propertyDescription, persistState)
        {
            _defaultValue = defaultValue;
        }

        protected override void BuildValue(IDTSCustomProperty100 property)
        {
            property.Value = _defaultValue;
        }
    }
}