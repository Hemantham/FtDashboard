using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dashboard.API.Enums;

namespace Dashboard.API.Domain
{
    public class DashboardView : DataEntity
    {
        public string Name { get; set; }
        public string Code { get; set; }
        public ICollection<Question> FieldOfInterest { get; set; }
        public string XAxislable { get; set; }
        public string XAxisId { get; set; }
        public DashboardView Parent { get; set; }
        public  short ViewOrder { get; set; }
        public ICollection<DashboardView> ChildrenViews { get; set; }
        public ICollection<ProductView> ProductViews { get; set; }
        [JsonConverter(typeof(StringEnumConverter))]
        public ChartRenderType ChartRenderType { get; set; }
        [JsonConverter(typeof(StringEnumConverter))]
        public DataAnlysisType DataAnlysisType { get; set; }
        public string  ChartRanges { get; set; }

    }
}
