using Microsoft.SqlServer.Dts.Pipeline.Wrapper;
using SsisComponents.Transformations.CustomProperties.Abstract;

namespace SsisComponents.Transformations.CustomProperties.Concrete
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