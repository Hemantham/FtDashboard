using System;
using System.Collections.Generic;
using System.Linq;
using Dashboard.API.API;
using Dashboard.API.Domain;
using Dashboard.Models;
using DataEf.Context;

namespace Dashboard.Services
{
    public class ChartDataService : IChartDataService
    {
        private readonly EfUnitOfWork _unitOfWork;

        public ChartDataService(EfUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        //todo : performance
        public IEnumerable<ChartEntry> GetChartEntries(ChartSearchCriteria criteria)
        {
            var responses = _unitOfWork.GetRepository<Response>()
                .Include(response => response.Question)
                .Get();

            var responsesGroupes = responses.GroupBy(r => r.ResponseId); // create a group for each response 

            //within that response check if we have given filters. 
            //filter by the cuts/filters , etc
            var filteredResponsesGroupes = responsesGroupes
                .Where(rg =>
                        criteria.Filters.All(q=> 
                                rg.Any(r => r.Question.Code == q.Code && r.Answer == q.Answer)))
                .ToList();

            var datafields = filteredResponsesGroupes.Select(rg =>
                    new
                    {
                        Data = rg.Any() ? rg.First(r => r.Question.Code == "CHRUN1").Answer : string.Empty, //select the field we are interested in for charting,
                        Id = rg.Key,
                        XAxisLable = rg.Any(r => r.Question.Code == "ANALYSED_Week") ? rg.First(r => r.Question.Code == "ANALYSED_Week").Answer : string.Empty,
                        XAxisId = rg.Any(r => r.Question.Code == "ANALYSED_Week_#") ? long.Parse(rg.First(r => r.Question.Code == "ANALYSED_Week_#").Answer) : 0
                    })
                .GroupBy(d => d.XAxisId).ToList();

            return datafields.SelectMany(xg =>
                        xg.GroupBy(vg => vg.Data)
                          .Select(vg => new ChartEntry
                          {
                              Value = vg.Count() * 100 / xg.Count(),
                              XAxisLable = vg.Any() ? vg.First().XAxisLable : string.Empty,
                              XAxisId = vg.Any() ? vg.First().XAxisId : 0,
                              Series = vg.Key,
                          })
               ).ToList();
        }
    }
}
