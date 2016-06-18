using System;
using Microsoft.SqlServer.Dts.Pipeline.Wrapper;

namespace SsisComponents.Base.CustomProperties.Abstract
{
    public abstract class BaseCustomPropertyBuilder : ICustomPropertyBuilder
    {
        public string PropertyName { get; set; }
        public string PropertyDescription { get; set; }
        public DTSPersistState PersistState { get; set; }
        public object Value { get; set; }

        public IDTSCustomProperty100 Build(IDTSCustomPropertyCollection100 collection)
        {
            return BuildBasicProperty(collection);
        }

        private IDTSCustomProperty100 BuildBasicProperty(IDTSCustomPropertyCollection100 collection)
        {
            var property = collection.New();
            property.Name = PropertyName;
            property.Description = PropertyDescription;
            property.State = PersistState;
            property.Value = Value;

            return property;
        }
    }
}
