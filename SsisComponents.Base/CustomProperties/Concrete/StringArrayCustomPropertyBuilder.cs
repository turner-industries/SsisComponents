using Microsoft.SqlServer.Dts.Pipeline.Wrapper;
using SsisComponents.Base.CustomProperties.Abstract;

namespace SsisComponents.Base.CustomProperties.Concrete
{
    public class StringArrayCustomPropertyBuilder : BaseCustomPropertyBuilder
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
