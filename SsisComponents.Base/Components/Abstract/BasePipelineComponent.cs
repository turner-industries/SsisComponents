using Microsoft.SqlServer.Dts.Pipeline;
using Microsoft.SqlServer.Dts.Pipeline.Wrapper;
using SsisComponents.Base.Adapters.Abstract;
using SsisComponents.Base.Adapters.Concrete;

namespace SsisComponents.Base.Components.Abstract
{
    public class BasePipelineComponent : PipelineComponent
    {
        protected IComponentMetadataAdapter MetadataAdapter { get; private set; }
        protected IComComponentAdapter BaseComponentComAdapter {get; private set; }

        public BasePipelineComponent(
            IComponentMetadataAdapter metadataAdapter,
            IComComponentAdapter comComponentAdapter)
        {
            MetadataAdapter = metadataAdapter;
            BaseComponentComAdapter = comComponentAdapter;
        }

        public BasePipelineComponent()
        {
            MetadataAdapter = new ComponentMetadataAdapter();
        }

        public override void ProvideComponentProperties()
        {
            BaseComponentComAdapter.ProvideComponentProperties();
            BaseComponentComAdapter.ReinitializeMetaData();
        }

        public override void PreExecute()
        {
            BaseComponentComAdapter.PreExecute();
            BaseComponentComAdapter.ReinitializeMetaData();
        }

        public override void OnInputPathAttached(int inputID)
        {
            BaseComponentComAdapter.OnInputPathAttached(inputID);
            BaseComponentComAdapter.ReinitializeMetaData();
        }

        public override void ReinitializeMetaData()
        {
            BaseComponentComAdapter.ReinitializeMetaData();
            MetadataAdapter.Initialize(ComponentMetaData);
        }

        protected IDTSInputColumnCollection100 GetInputColumns(IDTSInput100 input = null) => 
            (input ?? ComponentMetaData.InputCollection[0]).InputColumnCollection;

        protected IDTSInput100 GetInput() => ComponentMetaData.InputCollection[0];
    }
}
