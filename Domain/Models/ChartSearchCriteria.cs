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
        public RecencyTypes RecencyType { get; set; }
        public IEnumerable<XAxis> SelectedRecencies { get; set; }
    }


    //public class FieldSearchCriteria
    //{
    //    public int ProductViewId { get; set; }
    //    public string QuestionCode { get; set; }
    //}
}
