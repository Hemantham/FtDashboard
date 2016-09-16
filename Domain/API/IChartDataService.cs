using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dashboard.API.Domain;
using Dashboard.API.Models;
using Dashboard.Models;
using ChartEntry = Dashboard.API.Models.ChartEntry;

namespace Dashboard.API.API
{
    public interface IChartDataService
    {
        IEnumerable<DataChart> GetChartValues(ChartSearchCriteria criteria);

        IEnumerable<FieldValueModel> GetFieldValues(int productViewId);

        IEnumerable<RecencyType> GetRecencyTypes();

    }
}
