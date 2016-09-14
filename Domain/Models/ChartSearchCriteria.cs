using System.Collections.Generic;
using Dashboard.API.Domain;
using Dashboard.API.Enums;
using Dashboard.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Dashboard.API.Models
{
    public class ChartSearchCriteria
    {
        public ViewSplit SelectedSplit { get; set; }
        public IEnumerable<string> SplitFilters { get; set; }
        public int  ProductViewId { get; set; }
        [JsonConverter(typeof(StringEnumConverter))]
        public RecencyType RecencyType { get; set; }
    }


    //public class FieldSearchCriteria
    //{
    //    public int ProductViewId { get; set; }
    //    public string QuestionCode { get; set; }
    //}
}
