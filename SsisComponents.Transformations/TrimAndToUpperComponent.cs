using System.Collections.Generic;
using System.Linq;
using Microsoft.SqlServer.Dts.Pipeline;
using Microsoft.SqlServer.Dts.Pipeline.Wrapper;
using Microsoft.SqlServer.Dts.Runtime.Wrapper;
using SsisComponents.Base.Components.Abstract;
using SsisComponents.Base.CustomProperties.Concrete;

namespace SsisComponents.Transformations
{
    [DtsPipelineComponent(
       DisplayName = "Trim and ToUpper",
       Description = "Calls Trim() and ToUpper() on the provided array of columns",
       ComponentType = ComponentType.Transform,
       IconResource = "SsisComponents.Transformations.Resources.Icon1.ico"
       )]
    public class TrimAndToUpperComponent : BasePipelineComponent
    {
        private readonly string _outputColumnPrefix = "Trimmed and Uppercased";
        private readonly Dictionary<int, int> _sourceToDestinationColumnMaping = new Dictionary<int, int>();

        private bool _trim;
        private bool _toUpper;

        // Note: a non-default constructor will cause the component to throw a design-time exception about missing
        // interfaces or not being registered

        public override void ProvideComponentProperties()
        {
            base.ProvideComponentProperties();

            MetadataAdapter.AddNewCustomDesignTimeProperty(
                new BooleanCustomPropertyBuilder(true)
                {
                    PropertyName = "Trim",
                    PropertyDescription = "Indicates whether or not to Trim() the column data",
                    PersistState = DTSPersistState.PS_DEFAULT
                });

            MetadataAdapter.AddNewCustomDesignTimeProperty(
                new BooleanCustomPropertyBuilder(true)
                {
                    PropertyName = "Uppercase",
                    PropertyDescription = "Indicates whether or not ToUpper() should be performed on the column data",
                    PersistState = DTSPersistState.PS_DEFAULT
                });
        }

        // Design time.  This event is fired whenever the user attaches a new input to the component.
        public override void OnInputPathAttached(int inputID)
        {
            base.OnInputPathAttached(inputID);

            MetadataAdapter.CheckAllInputColumns(inputID, DataType.DT_STR, DataType.DT_WSTR);

            var inputColumns = MetadataAdapter.GetInputColumnsByInputId(inputID);
            foreach (var inputColumn in inputColumns)
            {
                var destinationOuputColumn = MetadataAdapter
                    .CreateOutputColumnFromInputColumn($"{_outputColumnPrefix} {inputColumn.Name}", inputColumn);

                MetadataAdapter.AddNewCustomPropertyToOutputColumn(
                    destinationOuputColumn, 
                    "Source Column",
                    inputColumn.Name);
            }
        }

        // Design and run time.  At design time, this event fires anytime the component's editor is opened,
        // closed, or any of the values within it are changed.
        // During run time, this event is fired only once before PreExecute() and ProcessInput() to ensure that the component
        // is in a valid state.
        public override DTSValidationStatus Validate()
        {
            ReinitializeMetaData();

            _trim = MetadataAdapter.GetValueOfCustomProperty<bool>("Trim");
            _toUpper = MetadataAdapter.GetValueOfCustomProperty<bool>("Uppercase");

            if (!_trim && !_toUpper)
            {
                return DTSValidationStatus.VS_ISBROKEN;
            }

            return base.Validate();
        }

        // Run time.  This event is fired only once before ProcessInput() and allows for any setup that needs to be performed
        // before input rows begin processing.
        public override void PreExecute()
        {
            base.PreExecute();

            var outputColumns = MetadataAdapter.GetOutputColumns();

            foreach (var outputColumn in outputColumns)
            {
                var sourceColumnLineage = MetadataAdapter.GetCustomPropertyFromOutputColumn<string>(outputColumn, "Source Column");
                var inputColumn = MetadataAdapter.GetInputColumnByName(sourceColumnLineage);
                var inputColumnIndex = MetadataAdapter.GetInputColumnIndex(inputColumn);

                _sourceToDestinationColumnMaping.Add(inputColumnIndex, 
                    MetadataAdapter.GetOutputColumnIndex(outputColumn) + MetadataAdapter.GetInputColumns().Count());
            }
        }

        // Run time.  This event fires for every row in the input.  The buffer argument contains all of the columns from
        // both input and output collections, with the input columns listed first.  All columns are in top-down order
        // as they are listed in the "Input and Output Properties" tab of the component's editor.  If you need
        // access to the output columns, you must then factor in the number of input columns.
        public override void ProcessInput(int inputID, PipelineBuffer buffer)
        {
            ReinitializeMetaData();

            while (buffer.NextRow())
            {
                foreach (var mapping in _sourceToDestinationColumnMaping)
                {
                    var columnValue = buffer.GetString(mapping.Key);

                    if (_trim)
                    {
                        columnValue = columnValue?.Trim();
                    }

                    if (_toUpper)
                    {
                        columnValue = columnValue?.ToUpper();
                    }

                    buffer.SetString(mapping.Value, columnValue);
                }
            }
        }
    }
}
