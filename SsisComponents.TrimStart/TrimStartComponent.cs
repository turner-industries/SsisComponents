using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.SqlServer.Dts.Pipeline;
using Microsoft.SqlServer.Dts.Pipeline.Wrapper;
using Microsoft.SqlServer.Dts.Runtime.Wrapper;
using SsisComponents.Base.Components.Abstract;
using SsisComponents.Base.CustomProperties.Concrete;

namespace SsisComponents.TrimStart
{
    [DtsPipelineComponent(
       DisplayName = "Trim Start",
       Description = "Calls TrimStart() on column data, allowing for various strings and columns to be passed as arguments.  Whitespace is trimmed by default.",
       ComponentType = ComponentType.Transform,
       IconResource = "SsisComponents.Transformations.Resources.Icon1.ico"
       )]
    public class TrimStartComponent : BasePipelineComponent
    {
        private const string _stringToTrimColumnName = "String to Trim Column";
        private const string _stringToRemoveColumnName = "String to Remove Column";

        // First int:   index of the column containing the string to remove
        // Second int:  index of the column containing the string to be trimmed
        // Third int:   index of the output column that will receive newly trimmed string
        private readonly List<Tuple<int, int, int>> _outputColumnAndSourceInputLineageColumnIds = new List<Tuple<int, int, int>>();

        private string[] _stringsToTrim;
        private bool _shouldProcess;

        public override void ProvideComponentProperties()
        {
            base.ProvideComponentProperties();

            MetadataAdapter.AddNewCustomDesignTimeProperty(
                new StringArrayCustomPropertyBuilder(
                    "Strings to trim",
                    "List of strings and characters to trim from the front of the column data.  Whitespace is trimmed by " +
                        "default and will be trimmed between every string in this list.  If a source column is specified in " +
                        "the output properties, it will be trimmed before any of the items in this list.",
                    DTSPersistState.PS_PERSISTASCDATA)
                );
        }

        public override IDTSOutputColumn100 InsertOutputColumnAt(int outputID, int outputColumnIndex, string name, string description)
        {
            var outputColumn = base.InsertOutputColumnAt(outputID, outputColumnIndex, name, description);
            outputColumn.SetDataTypeProperties(
                DataType.DT_WSTR, 4000, 0, 0, 0);

            MetadataAdapter.AddNewCustomPropertyToOutputColumn(
                outputColumn,
                _stringToTrimColumnName,
                string.Empty);

            MetadataAdapter.AddNewCustomPropertyToOutputColumn(
                outputColumn,
                _stringToRemoveColumnName,
                string.Empty);

            return outputColumn;
        }

        public override void PreExecute()
        {
            base.PreExecute();

            _stringsToTrim = MetadataAdapter.GetValueOfCustomProperty<string[]>("Strings to trim");
            var outputColumns = MetadataAdapter.GetOutputColumns()
                .Select(c => new
                {
                    OutputColumn = c,
                    StringToTrimColumnName = MetadataAdapter.GetCustomPropertyFromOutputColumn<string>(c, _stringToTrimColumnName),
                    StringToRemoveColumnName = MetadataAdapter.GetCustomPropertyFromOutputColumn<string>(c, _stringToRemoveColumnName)

                })
                .Where(c => !string.IsNullOrEmpty(c.StringToTrimColumnName) && !string.IsNullOrEmpty(c.StringToRemoveColumnName))
                .Select(c => new
                {
                    c.OutputColumn,
                    StringToTrimColumn = MetadataAdapter.GetInputColumnByName(c.StringToTrimColumnName) ,
                    StringToRemoveColumn = MetadataAdapter.GetInputColumnByName(c.StringToRemoveColumnName)
                });

            foreach (var outputColumn in outputColumns)
            {
                _outputColumnAndSourceInputLineageColumnIds.Add(new Tuple<int, int, int>(
                    MetadataAdapter.GetInputColumnIndex(outputColumn.StringToRemoveColumn),
                    MetadataAdapter.GetInputColumnIndex(outputColumn.StringToTrimColumn),
                    MetadataAdapter.GetOutputColumnIndex(outputColumn.OutputColumn) + MetadataAdapter.GetInputColumns().Count()
                    ));
            }

            _shouldProcess = _stringsToTrim.Any() || _outputColumnAndSourceInputLineageColumnIds.Any();
        }

        public override void ProcessInput(int inputID, PipelineBuffer buffer)
        {
            if (_shouldProcess)
            {
                ReinitializeMetaData();

                while (buffer.NextRow())
                {
                    foreach (var mapping in _outputColumnAndSourceInputLineageColumnIds)
                    {
                        var stringToTrim = buffer.GetString(mapping.Item2);
                        var stringToRemove = buffer.GetString(mapping.Item1);
                        var startsWith = stringToTrim?.StartsWith(stringToRemove);

                        if (startsWith.HasValue && startsWith.Value)
                        {
                            stringToTrim = stringToTrim?.Substring(stringToRemove.Length);
                        }

                        stringToTrim = stringToTrim?.TrimStart(' ');

                        foreach (var s in _stringsToTrim)
                        {
                            if (s.Length == 1)
                            {
                                stringToTrim = stringToTrim?.TrimStart(s.ToCharArray());
                            }
                            else
                            {
                                stringToTrim = stringToTrim?.Substring(s.Length);
                            }

                            stringToTrim = stringToTrim?.TrimStart(' ');
                        }

                        buffer.SetString(mapping.Item3, stringToTrim);
                    }
                }
            }
        }
    }
}

