using System.Collections.Generic;
using Dashboard.API.Domain;
using Dashboard.API.Enums;
using Dashboard.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Dashboard.API.Models
{
    public class ComparisonChartSearchCriteria
    {
       // public ViewSplit SelectedSplit { get; set; }
       // public IEnumerable<string> SplitFilters { get; set; }
        public int DashboardViewId { get; set; }
        public RecencyTypes RecencyType { get; set; }
        public IEnumerable<XAxis> SelectedRecencies { get; set; }
        //public IEnumerable<int> ProductsIds { get; set; }
    }
  
}
