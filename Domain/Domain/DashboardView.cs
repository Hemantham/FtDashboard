using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        [JsonConverter(typeof(StringEnumConverter))]
        public ChartRenderType ChartRenderType { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public DataAnlysisType DataAnlysisType { get; set; }

    }

    public enum ChartRenderType : short
    {
      line = 0,
      bar = 1,
    }

    public enum DataAnlysisType : short
    {
        percentage = 0,
        avarage = 1,
    }
}
