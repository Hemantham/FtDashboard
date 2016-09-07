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
        public IEnumerable<ChartEntry> GetChartValues(ChartSearchCriteria criteria)
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
                        Data = rg.FirstOrDefault(r => r.Question.Code == criteria.FieldOfInterest)?.Answer, //select the field we are interested in for charting,
                        Id = rg.Key,
                        XAxisLable =  rg.FirstOrDefault(r => r.Question.Code == criteria.XAxislable)?.Answer ,
                        XAxisId = rg.Any(r => r.Question.Code == criteria.XAxisId) ? long.Parse(rg.First(r => r.Question.Code == criteria.XAxisId).Answer) : 0
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
               )
               .OrderBy(df=>df.XAxisId)
               .ToList();
        }

        public IEnumerable<Response> GetFieldValues(string questionCode)
        {
            return  _unitOfWork
                .GetRepository<Response>()
                .Include(r => r.Question)
                .Get(r=> r.Question.Code == questionCode)
                .GroupBy(r => r.Answer)
                .Select(rg => rg.FirstOrDefault())
                .Where(r=> r!= null);
        }
    }
}
