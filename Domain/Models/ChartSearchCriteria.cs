using System.Collections.Generic;
using Dashboard.API.Domain;
using Dashboard.Models;

namespace Dashboard.API.Models
{
    public class ChartSearchCriteria
    {
        public ViewSplit SelectedSplit { get; set; }
        public IEnumerable<string> SplitFilters { get; set; }
        public int  ProductViewId { get; set; }
    }


    public class FieldSearchCriteria
    {
        public int ProductViewId { get; set; }
        public string QuestionCode { get; set; }
    }
}
