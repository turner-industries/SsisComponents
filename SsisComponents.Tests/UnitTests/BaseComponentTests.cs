using Microsoft.SqlServer.Dts.Pipeline.Wrapper;
using Moq;
using SsisComponents.Base.Adapters.Abstract;
using SsisComponents.Base.Components.Abstract;
using System;
using Xunit;

namespace SsisComponents.Tests.UnitTests
{
    public class BaseComponentTests : IDisposable
    {
        private readonly Mock<IComponentMetadataAdapter> _metaDataAdapter;
        private readonly Mock<IComComponentAdapter> _baseComponentComAdapter;

        public BaseComponentTests()
        {
            _metaDataAdapter = new Mock<IComponentMetadataAdapter>();
            _baseComponentComAdapter = new Mock<IComComponentAdapter>();
        }

        [Fact]
        public void BaseComponent_PreExecute_Succeeds()
        {
            var baseComponent = new BasePipelineComponent(_metaDataAdapter.Object, _baseComponentComAdapter.Object);

            baseComponent.PreExecute();

            _baseComponentComAdapter
                .Verify(c => c.PreExecute(), Times.Once());

            _baseComponentComAdapter
                .Verify(c => c.ReinitializeMetaData(), Times.Once());
        }

        [Fact]
        public void BaseComponent_ProvideComponentProperties_Succeeds()
        {
            var baseComponent = new BasePipelineComponent(_metaDataAdapter.Object, _baseComponentComAdapter.Object);

            baseComponent.ProvideComponentProperties();

            _baseComponentComAdapter
                .Verify(c => c.ProvideComponentProperties(), Times.Once());
            _baseComponentComAdapter
                .Verify(c => c.ReinitializeMetaData(), Times.Once());
        }

        [Fact]
        public void BaseComponent_OnInputPathAttached_Succeeds()
        {
            var baseComponent = new BasePipelineComponent(_metaDataAdapter.Object, _baseComponentComAdapter.Object);

            baseComponent.OnInputPathAttached(0);

            _baseComponentComAdapter
                .Verify(c => c.OnInputPathAttached(0), Times.Once());
            _baseComponentComAdapter
                .Verify(c => c.ReinitializeMetaData(), Times.Once());
        }

        [Fact]
        public void BaseComponent_ReinitializeMetaData_Succeeds()
        {
            var baseComponent = new BasePipelineComponent(_metaDataAdapter.Object, _baseComponentComAdapter.Object);

            baseComponent.ReinitializeMetaData();

            _baseComponentComAdapter
                .Verify(c => c.ReinitializeMetaData(), Times.Once());

            _metaDataAdapter
                .Verify(c => c.Initialize(It.IsAny<IDTSComponentMetaData100>()), Times.Once());
        }

        public void Dispose()
        {
        }
    }
}
