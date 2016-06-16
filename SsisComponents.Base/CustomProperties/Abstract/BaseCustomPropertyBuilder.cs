using Microsoft.SqlServer.Dts.Pipeline.Wrapper;

namespace SsisComponents.Base.CustomProperties.Abstract
{
    public abstract class BaseCustomPropertyBuilder : ICustomPropertyBuilder
    {
        protected readonly string PropertyName;
        protected readonly string PropertyDescription;
        protected readonly DTSPersistState PersistState;

        protected BaseCustomPropertyBuilder(
            string propertyName,
            string propertyDescription,
            DTSPersistState persistState)
        {
            PropertyName = propertyName;
            PropertyDescription = propertyDescription;
            PersistState = persistState;
        }

        public IDTSCustomProperty100 Build(IDTSCustomPropertyCollection100 collection)
        {
            var property = BuildBasicProperty(collection);

            BuildValue(property);

            return property;
        }

        protected abstract void BuildValue(IDTSCustomProperty100 property);

        private IDTSCustomProperty100 BuildBasicProperty(IDTSCustomPropertyCollection100 collection)
        {
            var property = collection.New();
            property.Name = PropertyName;
            property.Description = PropertyDescription;
            property.State = PersistState;

            return property;
        }
    }
}
