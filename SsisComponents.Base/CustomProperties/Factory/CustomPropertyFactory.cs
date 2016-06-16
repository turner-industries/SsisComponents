using Microsoft.SqlServer.Dts.Pipeline.Wrapper;
using SsisComponents.Transformations.CustomProperties.Abstract;

namespace SsisComponents.Transformations.CustomProperties.Factory
{
    public class CustomPropertyFactory
    {
        private readonly IDTSCustomPropertyCollection100 _collection;

        public CustomPropertyFactory(IDTSCustomPropertyCollection100 collection)
        {
            _collection = collection;
        }

        public IDTSCustomProperty100 Create(ICustomPropertyBuilder builder)
        {
            return builder.Build(_collection);
        }
    }
}
