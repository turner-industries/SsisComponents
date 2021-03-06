﻿using Microsoft.SqlServer.Dts.Pipeline;
using Microsoft.SqlServer.Dts.Pipeline.Wrapper;
using SsisComponents.Base.Adapters.Abstract;
using SsisComponents.Base.Adapters.Concrete;

namespace SsisComponents.Base.Components.Abstract
{
    public class BasePipelineComponent : PipelineComponent
    {
        protected IComponentMetadataAdapter MetadataAdapter { get; private set; }

        public BasePipelineComponent(
            IComponentMetadataAdapter metadataAdapter)
        {
            MetadataAdapter = metadataAdapter;
        }

        public BasePipelineComponent()
        {
            MetadataAdapter = new ComponentMetadataAdapter();
        }

        public override void ProvideComponentProperties()
        {
            base.ProvideComponentProperties();
            ReinitializeMetaData();
        }

        public override void PreExecute()
        {
            base.PreExecute();
            ReinitializeMetaData();
        }

        public override void OnInputPathAttached(int inputID)
        {
            base.OnInputPathAttached(inputID);
            ReinitializeMetaData();
        }

        public override void ReinitializeMetaData()
        {
            base.ReinitializeMetaData();
            MetadataAdapter.Initialize(ComponentMetaData);
        }

        protected IDTSInputColumnCollection100 GetInputColumns(IDTSInput100 input = null) => 
            (input ?? ComponentMetaData.InputCollection[0]).InputColumnCollection;

        protected IDTSInput100 GetInput() => ComponentMetaData.InputCollection[0];
    }
}
