using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dashboard.API.Enums;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Dashboard.API.Domain
{
    public class ViewSplit : DataEntity
    {
        public Question Question { get; set; }
        public string SplitName { get; set; }
        public Filter Filter { get; set; } //Optional

        [JsonConverter(typeof(StringEnumConverter))]
        public SplitType SplitType { get; set; }
    }
}
