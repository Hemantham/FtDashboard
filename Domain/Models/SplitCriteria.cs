using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dashboard.API.Enums;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Dashboard.API.Models
{
    public class SplitCriteria
    {
        public long ViewSplitId { get; set; }
        public IEnumerable<object> SplitFilters { get; set; }
        [JsonConverter(typeof(StringEnumConverter))]
        public SplitType SplitType { get; set; }

    }
}
