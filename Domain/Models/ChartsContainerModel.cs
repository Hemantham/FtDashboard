using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dashboard.API.Enums;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Dashboard.API.Models
{
    public class ChartsContainerModel
    {
        public IEnumerable<DataChart> Charts { get; set; }
        public IEnumerable<XAxis> AvailableRecencies { get; set; }

        public IEnumerable<string> AvailableSeries { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public ChartRenderType ChartRenderType { get; set; }
    }
}
