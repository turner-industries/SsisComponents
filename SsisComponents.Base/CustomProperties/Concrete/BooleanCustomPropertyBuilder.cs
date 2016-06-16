using Microsoft.SqlServer.Dts.Pipeline.Wrapper;
using SsisComponents.Base.CustomProperties.Abstract;

namespace SsisComponents.Base.CustomProperties.Concrete
{
    public class BooleanCustomPropertyBuilder : BaseCustomPropertyBuilder
    {
        private readonly bool _initialState;

        public BooleanCustomPropertyBuilder(
            string propertyName,
            string propertyDescription,
            DTSPersistState persistState,
            bool initialState) :
            base(propertyName, propertyDescription, persistState)
        {
            _initialState = initialState;
        }

        protected override void BuildValue(IDTSCustomProperty100 property)
        {
            property.Value = _initialState;
        }
    }
}