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
        public long  ProductViewId { get; set; } 
        public long DashboardViewId { get; set; } // if ProductViewId is not given then it will pick the overall based on DashboardViewId
        public RecencyTypes RecencyType { get; set; }
        public IEnumerable<XAxis> SelectedRecencies { get; set; }
        public bool UseFilterName { get; set; }
        public IEnumerable<SplitCriteria> SplitCriteria { get; set; }
    }
}
