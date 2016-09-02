using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dashboard.Models;

namespace Dashboard.API.API
{
    public interface IChartDataService
    {
        IEnumerable<ChartEntry> GetChartValues(ChartSearchCriteria criteria);

    }
}
