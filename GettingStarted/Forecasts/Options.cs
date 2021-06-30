using System.Collections.Generic;
using CommandLine;
using CommandLine.Text;

namespace Forecasts
{
    public class Options
    {
        // Models a command line value.
        [Value(0, MetaName = "count", Required = true, HelpText = "The number of weather forecasts to display.")]
        public int Count { get; set; }

        // Usage provide meta data for help screen.
        [Usage(ApplicationAlias = "forecasts")]
        public static IEnumerable<Example> Examples => new List<Example>
        {
            new Example("Display local weather forecasts",
                new Options { Count = 5 })
        };
    }
}
