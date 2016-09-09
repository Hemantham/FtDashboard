using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Dashboard.API.Domain
{
    public class ViewSplit : DataEntity
    {
        public string SplitField { get; set; }
        public string SplitName { get; set; }
        public Filter Filter { get; set; } //Optional

        [JsonConverter(typeof(StringEnumConverter))]
        public SplitType SplitType { get; set; }
    }

    public enum SplitType : short
    {
        All = 0,
        Single = 1,
        Mutiple = 2,
    }
}
