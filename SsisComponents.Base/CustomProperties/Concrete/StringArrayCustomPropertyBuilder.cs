using Microsoft.SqlServer.Dts.Pipeline.Wrapper;
using SsisComponents.Transformations.CustomProperties.Abstract;

namespace SsisComponents.Transformations.CustomProperties.Concrete
{
    internal class StringArrayCustomPropertyBuilder : BaseCustomPropertyBuilder
    {
        public StringArrayCustomPropertyBuilder(
            string propertyName,
            string propertyDescription,
            DTSPersistState persistState) :
            base(propertyName, propertyDescription, persistState)
        {
        }

        protected override void BuildValue(IDTSCustomProperty100 property)
        {
            property.Value = new string[0];
        }
    }
}
